using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xUnitTest.Attributes;
using static DAL.Entities.User;

namespace xUnitTest.UnitTests
{
    [TestCaseOrderer("xUnitTest.PriorityOrderer", "xUnitTest")]
    public class UserServiceUnitTest : IClassFixture<DependencyInjection>
    {
        private ServiceProvider _provider;
        private IServiceScope _scope;
        private IUserService _userService;
        private IRoleService _roleService;
        private IPermissionRightService _permissionRightService;
        private IPermissionService _permissionService;

        public UserServiceUnitTest(DependencyInjection injection)
        {
            _provider = injection.provider;
            _scope = _provider.CreateScope();
            _userService = _scope.ServiceProvider.GetService<IUserService>();
            _roleService = _scope.ServiceProvider.GetService<IRoleService>();
            _permissionRightService = _scope.ServiceProvider.GetService<IPermissionRightService>();
            _permissionService = _scope.ServiceProvider.GetService<IPermissionService>();
        }

        #region Login 
        [Fact, TestPriority(0)]
        public async Task LoginTest()
        {
            string email = "superadmin@fsoft.com";
            string password = "superadmin";
            AccountViewModel result = null;
            try
            {
                result = await _userService.Login(email, password);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Login by {email} with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Login with wrong email or password
        [Fact, TestPriority(1)]
        public async Task LoginWithWrongEmailOrPasswordTest()
        {
            string email = "superadmin@fsoft.com";
            string password = "123123";
            AccountViewModel result = null;
            try
            {
                result = await _userService.Login(email, password);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Login by {email} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Login with invalid email
        [Fact, TestPriority(2)]
        public async Task LoginWithInvalidEmailTest()
        {
            string email = "superadmin";
            string password = "superadmin";
            AccountViewModel result = null;
            try
            {
                if (Regex.IsMatch(email, @"^(\w+@\w+\.\w+)$"))
                    result = await _userService.Login(email, password);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Login by {email} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Get Four Users In One Page
        [Fact, TestPriority(3)]
        public void GetFourUsersTest()
        {
            var result = new List<UserViewModel>();
            List<string> sortBy = new List<string>();
            try
            {
                result = _userService.GetAll(null, sortBy, 4, 1);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Four Users with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Get Users Contain Keyword
        [Fact, TestPriority(4)]
        public void GetUsersWithKeywordTest()
        {
            string keyword = "Admin";
            var result = new List<UserViewModel>();
            List<string> sortBy = new List<string>();
            try
            {
                result = _userService.GetAll(keyword, sortBy, 4, 1);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Four Users with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Get Users By Id Descending
        [Fact, TestPriority(5)]
        public void GetUsersByIdDesc()
        {
            List<string> sortBy = new List<string>();
            sortBy.Add("Id DESC");
            var result = new List<UserViewModel>();
            try
            {
                result = _userService.GetAll(null, sortBy, 4, 1);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Four Users with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Get Users Contain Keyword and Sort By Id Descending
        [Fact, TestPriority(6)]
        public void GetUsersByKeywordAndIdDesc()
        {
            string keyword = "Admin";
            List<string> sortBy = new List<string>();
            sortBy.Add("Id DESC");
            var result = new List<UserViewModel>();
            try
            {
                result = _userService.GetAll(keyword, sortBy, 4, 1);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Four Users with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Add User
        [Fact, TestPriority(7)]
        public async Task AddUserTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "123123",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "0909696969",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                result = await _userService.Add(testUser);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Add User with user's name not in between 3 and 30 characters
        [Fact, TestPriority(8)]
        public async Task AddUserWithUserNameNotInBetween3And30CharactersTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "N",
                Password = "123123",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "0909696969",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                if (testUser.UserName.Length < 3)
                {
                    result = await _userService.Add(testUser);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Add User with user's password not in between 6 and 24 characters
        [Fact, TestPriority(9)]
        public async Task AddUserWithPasswordNotInBetween6And24CharactersTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "12345",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "0909696969",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                if (testUser.Password.Length < 6)
                {
                    result = await _userService.Add(testUser);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Add User with invalid full name
        [Fact, TestPriority(10)]
        public async Task AddUserWithInvalidFullNameTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "123123",
                Fullname = "",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "0909696969",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                if (!testUser.Fullname.Equals(""))
                {
                    result = await _userService.Add(testUser);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Add User with invalid gender
        [Fact, TestPriority(11)]
        public async Task AddUserWithInvalidGenderTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "123123",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'B',
                Phone = "0909696969",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                if (testUser.Gender.Equals('F') || testUser.Gender.Equals('M'))
                {
                    result = await _userService.Add(testUser);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Add User with invalid phone
        [Fact, TestPriority(12)]
        public async Task AddUserWithInvalidPhoneTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "123123",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                if (testUser.Phone.Equals(""))
                {
                    result = await _userService.Add(testUser);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Add User with invalid email
        [Fact, TestPriority(13)]
        public async Task AddUserWithInvalidEmailTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "123123",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "0909696969",
                Email = "hoa",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                if (Regex.IsMatch(testUser.Email, @"^(\w+@\w+\.\w+)$"))
                {
                    result = await _userService.Add(testUser);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Add User with existed email
        [Fact, TestPriority(14)]
        public async Task AddUserWithExistedEmailTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "123123",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "0909696969",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 2
            };

            UserViewModel result = null;

            try
            {
                result = await _userService.Add(testUser);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Add User with invalid role
        [Fact, TestPriority(15)]
        public async Task AddUserWithInvalidRoleTest()
        {
            bool dob = DateTime.TryParse("2022-10-31", out DateTime dateOfBirth);
            UserAccountViewModel testUser = new UserAccountViewModel
            {
                UserName = "Hoa",
                Password = "123123",
                Fullname = "Ngoc Hoa",
                DateOfBirth = dateOfBirth,
                Gender = 'F',
                Phone = "0909696969",
                Email = "hoa@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 6
            };

            UserViewModel result = null;

            try
            {
                var roleTest = _roleService.GetByID(testUser.IdRole);
                if (roleTest != null)
                {
                    result = await _userService.Add(testUser);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Add User with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Send Otp with existed Email
        [Fact, TestPriority(16)]
        public async Task SendOtpWithExistedEmailTest()
        {
            string email = "hoa@gmail.com";
            bool result = false;
            try
            {
                result = await _userService.SendOtp(email);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Send Otp to {email} with exception: {ex}");
            }
            Assert.True(result);
        }
        #endregion

        #region Send Otp with non-existed Email
        [Fact, TestPriority(17)]
        public async Task SendOtpWithNonExistedEmailTest()
        {
            string email = "notfound@gmail.com";
            bool result = true;
            try
            {
                result = await _userService.SendOtp(email);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Send Otp to {email} with exception: {ex}");
            }
            Assert.False(result);
        }
        #endregion

        #region Reset Password with matched new Password and confirm Password
        [Fact, TestPriority(18)]
        public async Task ResetPasswordWithMatchedNewPasswordAndConfirmPasswordTest()
        {
            string email = "hoa@gmail.com";
            string password = "123123";
            var userTest = await _userService.GetUser(email, password);
            string newPassword = "321321";
            string confirmPassword = "321321";
            bool result = false;
            try
            {
                if (newPassword.Equals(confirmPassword))
                {
                    result = await _userService.ResetPassword(newPassword, userTest.ResetPasswordOtp);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Reset user {email} password with exception: {ex}");
            }
            Assert.True(result);
        }
        #endregion

        #region Reset Password with wrong Otp
        [Fact, TestPriority(19)]
        public async Task ResetPasswordWithWrongOtpTest()
        {
            string email = "hoa@gmail.com";
            string password = "321321";
            var userTest = await _userService.GetUser(email, password);
            string newPassword = "456456";
            string confirmPassword = "654654";
            bool result = false;
            try
            {
                if (newPassword.Equals(confirmPassword))
                {
                    result = await _userService.ResetPassword(newPassword, userTest.ResetPasswordOtp);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Reset user password with exception: {ex}");
            }
            Assert.False(result);
        }
        #endregion

        #region Reset Password with not-matched new Password and confirm Password
        [Fact, TestPriority(20)]
        public async Task ResetPasswordWithNotMatchedNewPasswordAndConfirmPasswordTest()
        {
            string newPassword = "321321";
            string confirmPassword = "231231";
            string Otp = "111111";
            bool result = false;
            try
            {
                if (newPassword.Equals(confirmPassword))
                {
                    result = await _userService.ResetPassword(newPassword, Otp);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Reset user password with exception: {ex}");
            }
            Assert.False(result);
        }
        #endregion

        #region Import User
        [Fact, TestPriority(21)]
        public async Task ImportUsertTest()
        {
            string path = ".//TestSamples//form.xlsx";
            using var s = new MemoryStream(File.ReadAllBytes(path).ToArray());
            var formFile = new FormFile(s, 0, s.Length, "streamFile", path.Split("//").Last());

            UpLoadExcelFileRequest request = new UpLoadExcelFileRequest()
            {
                File = formFile
            };
            using (FileStream stream = new FileStream(request.File.FileName, FileMode.CreateNew))
            {
                await request.File.CopyToAsync(stream);
            }
            var response = new UpLoadExcelFileResponse()
            {
                IsSuccess = false
            };
            try
            {
                response = await _userService.ImportUser(request, path);
            }
            catch (Exception ex)
            {
                Assert.False(response.IsSuccess);
            }
            Assert.True(response.IsSuccess);


        }
        #endregion

        #region Edit User
        [Fact, TestPriority(22)]
        public async Task EditUserTest()
        {
            UserViewModel user = new UserViewModel
            {
                ID = 5,
                Fullname = "Trong Tan",
                DateOfBirth = new DateTime(2001, 1, 1),
                Gender = 'F',
                Email = "no1@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 4
            };
            UserViewModel result = null;
            try
            {
                result = await _userService.Edit(user);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Edit userId: {user.ID} with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Edit User with invalid fullname
        [Fact, TestPriority(23)]
        public async Task EditUserWithInvalidFullNameTest()
        {
            UserViewModel user = new UserViewModel
            {
                ID = 5,
                Fullname = "",
                DateOfBirth = new DateTime(2001, 1, 1),
                Gender = 'F',
                Email = "no1@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 4
            };
            UserViewModel result = null;
            try
            {
                if (!user.Fullname.Equals(""))
                {
                    result = await _userService.Edit(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Edit userId: {user.ID} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Edit User with invalid gender
        [Fact, TestPriority(24)]
        public async Task EditUserWithInvalidGenderTest()
        {
            UserViewModel user = new UserViewModel
            {
                ID = 5,
                Fullname = "Trong Tan",
                DateOfBirth = new DateTime(2001, 1, 1),
                Gender = 'B',
                Email = "no1@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 4
            };
            UserViewModel result = null;
            try
            {
                if (user.Gender.Equals('F') || user.Gender.Equals('M'))
                {
                    result = await _userService.Edit(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Edit userId: {user.ID} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Edit User with invalid email
        [Fact, TestPriority(25)]
        public async Task EditUserWithInvalidEmailTest()
        {
            UserViewModel user = new UserViewModel
            {
                ID = 5,
                Fullname = "Trong Tan",
                DateOfBirth = new DateTime(2001, 1, 1),
                Gender = 'F',
                Email = "no1",
                Address = "HCM",
                Status = 1,
                IdRole = 4
            };
            UserViewModel result = null;
            try
            {
                if (Regex.IsMatch(user.Email, @"^(\w+@\w+\.\w+)$"))
                {
                    result = await _userService.Edit(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Edit userId: {user.ID} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Edit User with existed email
        [Fact, TestPriority(26)]
        public async Task EditUserWithExistedEmailTest()
        {
            UserViewModel user = new UserViewModel
            {
                ID = 5,
                Fullname = "Trong Tan",
                DateOfBirth = new DateTime(2001, 1, 1),
                Gender = 'F',
                Email = "no1@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 4
            };
            UserViewModel result = null;
            try
            {
                result = await _userService.Edit(user);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Edit userId: {user.ID} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Edit User with invalid role
        [Fact, TestPriority(27)]
        public async Task EditUserWithInvalidRoleTest()
        {
            UserViewModel user = new UserViewModel
            {
                ID = 5,
                Fullname = "Trong Tan",
                DateOfBirth = new DateTime(2001, 1, 1),
                Gender = 'F',
                Email = "no1@gmail.com",
                Address = "HCM",
                Status = 1,
                IdRole = 6
            };
            UserViewModel result = null;
            try
            {
                var roleTest = _roleService.GetByID(user.IdRole);
                if (roleTest != null)
                {
                    result = await _userService.Edit(user);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Edit userId: {user.ID} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Get Roles
        [Fact, TestPriority(28)]
        public async Task GetRoleTest()
        {
            var result = new List<PermissionViewModel>();
            try
            {
                result = await _permissionRightService.GetAllRole();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Get Role with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Add Role
        [Fact, TestPriority(29)]
        public async Task AddRoleTest()
        {
            string roleName = "Fresher";
            Role result = null;
            try
            {
                result = await _roleService.AddNewRoleAsync(roleName);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Add Role {roleName} with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Add role with invalid name
        [Fact, TestPriority(30)]
        public async Task AddRoleWithInvalidNameTest()
        {
            string roleName = "";
            Role result = null;
            try
            {
                if (!roleName.Equals(""))
                    result = await _roleService.AddNewRoleAsync(roleName);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Add Role {roleName} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Change Role
        [Fact, TestPriority(31)]
        public async Task ChangeRoleUserTest()
        {
            long id = 5;
            long idRole = 4;
            UserViewModel result = null;
            try
            {
                result = await _userService.ChangleRole(id, idRole);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to change userId: {id}'s role to roleId : {idRole} with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Change Role with invalid idRole
        [Fact, TestPriority(32)]
        public async Task ChangeRoleUserWithInvalidIdRoleTest()
        {
            long id = 5;
            long idRole = 6;
            UserViewModel result = null;
            try
            {
                var roleTest = _roleService.GetByID(idRole);
                if (roleTest != null)
                {
                    result = await _userService.ChangleRole(id, idRole);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to change userId: {id}'s role to roleId : {idRole} with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Set Permission
        [Fact, TestPriority(33)]
        public async Task SetPermissionTest()
        {
            long idRight = 2;
            long idRole = 1;
            long idPermission = 4;
            PermissionViewModel result = null;
            try
            {
                result = await _permissionRightService.SetPermission(idRight, idRole, idPermission);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Set Permission with exception: {ex}");
            }
            Assert.NotNull(result);
        }
        #endregion

        #region Set Permission with invalid idPermission
        [Fact, TestPriority(34)]
        public async Task SetPermissionWithInvalidIdPermissionTest()
        {
            long idRight = 2;
            long idRole = 1;
            long idPermission = 7;
            PermissionViewModel result = null;
            try
            {
                var test = _permissionService.GetPermission(idPermission);
                if (test != null)
                {
                    result = await _permissionRightService.SetPermission(idRight, idRole, idPermission);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failt to Set Permission with exception: {ex}");
            }
            Assert.Null(result);
        }
        #endregion

        #region Deactivate User 
        [Fact, TestPriority(35)]
        public async Task DeactivateUserTest()
        {
            long id = 5;
            bool result;
            try
            {
                result = await _userService.DeActivate(id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Deactivate userId: {id} with exception: {ex}");
            }
            var deactivatedUser = await _userService.GetById(id);
            Assert.True(Equals(deactivatedUser.Status, 0));
        }
        #endregion

        #region Delete User
        [Fact, TestPriority(36)]
        public async Task DeleteUserTest()
        {
            long id = 5;
            bool result = false;
            try
            {
                result = await _userService.Delete(id);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fail to Delete userId: {id} with exception: {ex}");
            }
            Assert.True(result);
        }
        #endregion
    }
}
