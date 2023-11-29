using Microsoft.AspNetCore.Mvc;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using BAL.Validators;
using System.Text.RegularExpressions;

namespace FRMAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private string _secretKey;
    private IPermissionRightService _permissionRightService;
    private IPermissionService _permissionService;
    private IRightService _rightService;
    private IRoleService _roleService;
    private IRefreshTokenService _refreshTokenService;

    public AuthController(IUserService userService, IUnitOfWork unitOfWork, IConfiguration configuration, IPermissionRightService permissionRightService,
        IPermissionService permissionService, IRightService rightService, IRoleService roleService, IRefreshTokenService refreshTokenService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _secretKey = configuration.GetSection("AppSettings:SerectKey").Value;
        _permissionRightService = permissionRightService;
        _permissionService = permissionService;
        _rightService = rightService;
        _roleService = roleService;
        _refreshTokenService = refreshTokenService;
    }
    #region Login
    /// <summary>
    /// UC0-001
    /// Log into system using email and password
    /// </summary>    
    /// <remarks>
    ///     Sample request:
    ///
    ///         {
    ///           "email": "superadmin@fsoft.com",
    ///           "password": "superadmin"
    ///         }
    ///         
    /// </remarks>
    /// <returns>Specific HTTP Status code</returns>
    /// <response code="200">Return home screen if the access is successful</response>
    /// <response code="400">If the account is null</response>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Boolean), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] AccountViewModel account)
    {
        string errorMessage = "";
        bool status = false;
        User user = null;
        TokenModel jwt = null;

        AccountValidator validator = new AccountValidator();

        try
        {
            if (account == null) throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");

            var validation = validator.Validate(account);
            if (!validation.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validation.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                return BadRequest(new
                {
                    Status = status,
                    Errors = errors
                });
            }

            user = await _userService.GetUser(account.Email, account.Password);
            if (user == null)
            {
                errorMessage = "Wrong Email or Password";
                return BadRequest(new
                {
                    Status = status,
                    ErrorMessage = errorMessage
                });
            }

            if (user.LoginTimeOut > DateTime.Now && user.LoginAttemps >= 3)
            {
                errorMessage = "Your account has been temporarily disabled. Please try after 15 minutes";
                return BadRequest(new
                {
                    Status = status, 
                    ErrorMessage = errorMessage
                });
            }
            jwt = await GenerateToken(user);
            status = true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }

        return Ok(new
        {
            Status = status,
            Data = jwt
        });
    }
    #endregion

    #region Group 5 - Authentication & Authorization
    private async Task<TokenModel> GenerateToken(User userViewModel)
    {
        userViewModel.Role = _roleService.GetRole(userViewModel.IdRole);
        // lấy list tên right - tên permission dưới db
        IEnumerable<PermissionRight> permissionRights = _permissionRightService.GetPermissionRightsByRoleId(userViewModel.IdRole).ToList();
        userViewModel.Role.PermissionRights = permissionRights;
        foreach (var permissionRight in permissionRights.ToList())
        {
            permissionRight.Permission = _permissionService.GetPermission(permissionRight.IdPermission);
            permissionRight.Right = _rightService.GetRight(permissionRight.IdRight);
        }
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userViewModel.UserName),
            new Claim(ClaimTypes.Email, userViewModel.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Id", userViewModel.ID.ToString()),
            new Claim(ClaimTypes.Role, userViewModel.Role.Name)
        };
        foreach (var permissionRight in permissionRights)
        {
            var newRightName = "";
            if (permissionRight.Right.Name.Contains(' '))
            {
                var rightName = permissionRight.Right.Name.Split(' ');
                for (int i = 0; i < rightName.Length; i++)
                {
                    newRightName += rightName[i];
                }
            }
            else
            {
                newRightName = permissionRight.Right.Name;
            }
            claims.Add(new Claim(newRightName.ToLower(), permissionRight.Permission.Name.ToLower()));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userViewModel.ID,
            JwtId = token.Id,
            Token = refreshToken,
            IsUsed = false,
            IsRevoked = false,
            IssuedAt = DateTime.Now,
            ExpiredAt = DateTime.Now.AddHours(1)
        };

        var result = await _refreshTokenService.Add(refreshTokenEntity);

        return new TokenModel
        {
            AccessToken = jwt,
            RefreshToken = refreshToken
        };
    }
    #endregion

    #region Send OTP
    /// <summary>
    /// UC0-002
    /// Input user's email in order to reset password
    /// </summary>
    /// <param name="email">Enter email to reset password</param>
    /// 
    /// <remarks>
    ///     Sample request:
    ///
    ///         "email": "example@gmail.com"
    ///             
    /// </remarks>
    /// <returns>Specific HTTP Status code</returns>
    /// <response code="200">Return login page</response>
    /// <response code="400">Confirm password not matched</response>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendOtpAsync(string email)
    {
        string errorMessage = "";
        bool status = false;
        try
        {
            if (!Regex.IsMatch(email, @"^(\w+@\w+\.\w+)$"))
            {
                return BadRequest(new
                {
                    status = status,
                    errorMessage = "Invalid Email address"
                });
            }
            status = await _userService.SendOtp(email);
            if (status)
            {
                return Ok(new
                {
                    status = status,
                    ErrorMessage = errorMessage,
                });
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }

        return BadRequest(new
        {
            status = status,
            ErrorMessage = "Something went wrong"
        });
    }
    #endregion

    #region Reset Password
    /// <summary>
    /// UC0-003
    /// Enter OTP to comfirm 
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///
    ///         {
    ///           "Otp": "Sent by Email",
    ///           "NewPassword": "123456",
    ///           "ConfirmPassword": "123456"
    ///         }
    ///                  
    /// </remarks>
    /// <returns>Specific HTTP Status code</returns>
    /// <response code="200">return comfirmPassword page</response>
    /// <response code="400">Wrong OTP</response>
    [HttpPost]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] PasswordViewModel passwordViewModel)
    {
        string errorMessage = "";
        bool status = false;
        ResetPasswordValidator validator = new ResetPasswordValidator();
        try
        {
            if (passwordViewModel == null) throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");

            var validation = validator.Validate(passwordViewModel);
            if (!validation.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in validation.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
                return BadRequest(new
                {
                    Status = status,
                    Errors = errors
                });
            }

            status = await _userService.ResetPassword(passwordViewModel.NewPassword, passwordViewModel.Otp);
            if (status)
            {
                return Ok(new
                {
                    status = status,
                    ErrorMessage = errorMessage,
                });
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }

        return BadRequest(new
        {
            status = status,
            ErrorMessage = "Something went wrong"
        });
    }
    #endregion

    #region Refresh Tokens
    /// <summary>
    /// UC0-005 Refresh Tokens
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        try
        {
            var result = await _refreshTokenService.FindToken(refreshToken);
            if (result == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Refresh token does not exist"
                });
            }

            if (result.IsUsed)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Refresh token has been used"
                });
            }

            if (result.IsRevoked)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Refresh token has been revoked"
                });
            }

            if (result.ExpiredAt < DateTime.Now)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Refresh token has expired"
                });
            }

            var status = await _refreshTokenService.ChangeStatus(result);

            var user = await _userService.GetById(result.UserId);

            var token = await GenerateToken(user);

            return Ok(new
            {
                Sucess = true,
                Message = "Refresh token successfully",
                Data = token
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message,
            });
        }
    }
    #endregion

    #region Logout
    /// <summary>
    /// UC0-005
    /// Log out system
    /// </summary>   
    /// <returns>Specific HTTP Status code</returns>
    /// <response code="204">Return login page</response>
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> LogoutAsync()
    {
        //Response.Cookies.Delete("jwt", new CookieOptions
        //{
        //    HttpOnly = true,
        //    SameSite = SameSiteMode.None,
        //    Secure = true
        //});
        //return NoContent();
        //var userName = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        //if (userName == null)
        //{
        //    return Unauthorized();
        //}
        //if (await _userService.InvalidateAccessToken(userName))
        //{
        //    return NoContent();
        //}
        string rawUserId = HttpContext.User.FindFirstValue("Id");

        if (!long.TryParse(rawUserId, out long id))
        {
            return Unauthorized();
        }

        await _refreshTokenService.DeleteAll(id);

        return NoContent();
    }
    #endregion  

    #region Functions
    private string GenerateRefreshToken()
    {
        var random = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(random);

            return Convert.ToBase64String(random);
        }
    }
    #endregion
}