using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Rewrite;
using System.Linq.Dynamic.Core;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Security.Cryptography;

namespace DAL.Repositories.Implements
{

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly FRMDbContext _dbContext;
        private readonly IRoleRepository _role;
        private readonly IUnitOfWork _unitOfWork;
        public UserRepository(IDbFactory dbFactory, IRoleRepository role, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _dbContext = dbFactory.Init();
            _role = role;
            _unitOfWork = unitOfWork;
        }

        #region Login
        public async Task<User> Login(string email, string password)
        {
            bool isValidPassword = false;

            var result = await _dbSet.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (result != null)
            {
                isValidPassword = BCrypt.Net.BCrypt.Verify(password, result.Password);

                if (isValidPassword)
                {
                    if (result.LoginTimeOut < DateTime.Now)
                    {
                        result.LoginAttemps = 0;
                        result.LoginTimeOut = DateTime.Now.AddDays(12);
                        await _dbContext.SaveChangesAsync();
                    }
                    return result;
                }
                else
                {
                    result.LoginAttemps += 1;
                    await _dbContext.SaveChangesAsync();
                }

                if (result.LoginAttemps == 3)
                {
                    result.LoginTimeOut = DateTime.Now.AddMinutes(15);
                    await _dbContext.SaveChangesAsync();
                    return result;
                }

                if (result.LoginAttemps > 3)
                    return result;
            }

            return null;
        }
        #endregion

        #region SendOtp
        public async Task<bool> SendOtp(string email)
        {
            var result = await _dbSet.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (result != null)
            {
                await SendEmail(result);
                return true;
            }
            return false;
        }
        #endregion

        #region ResetPassword
        public async Task<bool> ResetPassword(string newPassword, string Otp)
        {
            var result = await _dbSet.Where(x => x.ResetPasswordOtp == Otp).FirstOrDefaultAsync();
            if (result != null)
            {
                result.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                result.ResetPasswordOtp = null;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region GetAllUser       
        public IQueryable<User> GetAllUser(string? keyword, List<string> sortBy, int PAGE_SIZE, int PAGE_NUMBER)
        {
            IQueryable<User> result = null;
            if (sortBy.Count() == 0)
            {
                sortBy = null;
            }
            if (keyword == null && sortBy == null)
            {
                result = _dbSet.Skip(PAGE_SIZE * (PAGE_NUMBER - 1)).Take(PAGE_SIZE);
                return result;
            }
            if (keyword != null && sortBy == null)
            {
                result = _dbSet.Where(x => x.FullName.Contains(keyword)).OrderByDescending(x => x.DateOfBirth).Skip(PAGE_SIZE * (PAGE_NUMBER - 1)).Take(PAGE_SIZE);

                return result;
            }
            if (keyword == null && sortBy != null)
            {
                result = Sort(sortBy, PAGE_SIZE, PAGE_NUMBER);
                return result;
            }
            if (keyword != null && sortBy != null)
            {
                result = SortSearch(keyword, sortBy, PAGE_SIZE, PAGE_NUMBER);
                return result;
            }
            return _dbSet.Where(x => x.ID == 0);
        }
        #endregion

        #region GetById
        public async Task<User> GetById(long id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
        #endregion

        #region AddUser
        public async Task<User> Add(User user)
        {
            var result = await CheckExist(user.UserName, user.Email);
            if (result == null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.LoginAttemps = 0;
                user.LoginTimeOut = DateTime.Now.AddDays(12);
                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }

            return null;


        }
        #endregion

        #region ChangeRole
        public async Task<User> ChangleRole(long id, long IdRole)
        {
            var exist = await _dbSet.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (exist != null && exist.IdRole > 0)
            {
                exist.IdRole = IdRole;
                await _dbContext.SaveChangesAsync();
            }
            return exist;
        }
        #endregion

        #region DeActivate       
        public async Task<bool> DeActivate(long id)
        {
            var user = await _dbSet.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (user == null)
                return false;
            if (user.Status == 0)
            {
                user.Status = 1;
            }
            else
                user.Status = 0;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        #endregion

        #region EditUser
        public async Task<User> Edit(User user)
        {
            var exist = await _dbSet.Where(x => x.ID == user.ID).FirstOrDefaultAsync();
            var result = await CheckExist(user.Email);

            if (result != null)
                return null;

            if (exist != null)
            {
                exist.FullName = user.FullName;
                exist.Email = user.Email;
                exist.DateOfBirth = user.DateOfBirth;
                exist.Gender = user.Gender;
                exist.Address = user.Address;
                exist.Status = user.Status;
                exist.IdRole = user.IdRole;
                await _dbContext.SaveChangesAsync();
            }
            return exist;
        }
        #endregion

        #region DeleteUser
        public async Task<bool> Delete(long id)
        {
            var exist = await _dbSet.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (exist != null)
            {
                _dbSet.Remove(exist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Functions
        private async Task<User> CheckExist(string username, string email)
        {
            var result = await _dbSet.Where(x => x.UserName == username || x.Email == email).FirstOrDefaultAsync();
            return result;
        }
        private async Task<User> CheckExist(string email)
        {
            var result = await _dbSet.Where(x => x.Email == email).FirstOrDefaultAsync();
            return result;
        }
        private IQueryable<User> Sort(List<string> sortby, int PAGE_SIZE, int PAGE_NUMBER)
        {
            String sort = "";
            foreach (var item in sortby)
            {
                sort = sort + ", " + item;
            }
            sort = sort.TrimStart(',');
            sort = sort.TrimStart(' ');
            return _dbContext.Users.AsQueryable().OrderBy(sort).Skip(PAGE_SIZE * (PAGE_NUMBER - 1)).Take(PAGE_SIZE);

        }
        private IQueryable<User> SortSearch(string keyword, List<string>? sortby, int PAGE_SIZE, int PAGE_NUMBER)
        {
            String sort = "";
            foreach (var item in sortby)
            {
                sort = sort + ", " + item;
            }
            sort = sort.TrimStart(',');
            sort = sort.TrimStart(' ');
            return _dbContext.Users.Where(x => x.FullName.Contains(keyword)).AsQueryable().OrderBy(sort).Skip(PAGE_SIZE * (PAGE_NUMBER - 1)).Take(PAGE_SIZE);

        }

        public async Task<bool> CheckEdit(long userId)
        {
            var result = await _dbSet.Where(x => x.ID == userId).FirstOrDefaultAsync();
            if (result != null)
                return true;
            return false;
        }

        private async Task SendEmail(User user)
        {
            ChangeUserOtp(user);
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("testingemailnow123@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Reset password OTP";
            email.Body = new TextPart(TextFormat.Plain) { Text = user.ResetPasswordOtp };

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync("smtp.gmail.com", 465, true);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtp.AuthenticateAsync("testingemailnow123@gmail.com", "qsggmwhmshlpxgfb");

                await smtp.SendAsync(email);
            }
            catch
            {
                throw;
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }

        private void ChangeUserOtp(User user)
        {
            user.ResetPasswordOtp = GenerateRandomOtp();
            _unitOfWork.commitAsync();
        }

        private string GenerateRandomOtp()
        {
            return Convert.ToString(RandomNumberGenerator.GetInt32(100000, 1000000));
        }
        #endregion     

        #region Other groups
        public User GetUser(long id)
        {
            return _dbSet.FirstOrDefault(u => u.ID == id);
        }

        public User GetUserById(long UserId)
        {
            return _dbSet.Where(u => u.ID == UserId).FirstOrDefault();
        }
        public User Get(long id)
        {
            return _dbSet.FirstOrDefault(x => x.ID == id);
        }

        public async Task<IEnumerable<User>> GetTrainers()
        {
            return await _dbSet.Include(u => u.Role).Where(t => t.Role.Name.Equals("Trainer")).ToListAsync();
        }

        #region Group 5 - Authentication & Authorization
        public User GetUser(string username)
        {
            return this._dbSet.FirstOrDefault(x => x.UserName.Equals(username));
        }
        public List<User> GetUsers(long id)
        {
            return _dbSet.Where(u => u.ID == id).ToList();
        }
        #endregion

        public async Task<User> GetUserAsync(long userId)
        {
            return await _dbSet.Include(x => x.Role).FirstOrDefaultAsync(x => x.ID == userId);
        }

        // Team6
        public async Task<User> GetUser(string username, string password)
        {
            bool isValidPassword = false;

            var result = await _dbSet.FirstOrDefaultAsync(x => x.Email.Equals(username));

            if (result != null)
            {
                isValidPassword = BCrypt.Net.BCrypt.Verify(password, result.Password);

                if (isValidPassword)
                {
                    if (result.LoginTimeOut < DateTime.Now)
                    {
                        result.LoginAttemps = 0;
                        result.LoginTimeOut = DateTime.Now.AddDays(12);
                        await _dbContext.SaveChangesAsync();
                    }
                    return result;                  
                }
                else
                {
                    result.LoginAttemps += 1;
                    await _dbContext.SaveChangesAsync();
                }

                if (result.LoginAttemps == 3)
                {
                    result.LoginTimeOut = DateTime.Now.AddMinutes(15);
                    await _dbContext.SaveChangesAsync();
                    return result;
                }

                if (result.LoginAttemps > 3)
                    return result;
            }

            return null;
        }

        public List<User> GetUsersForImport()
        {
            return this._dbSet.ToList();
        }

        //team4
        public void CreateUserForImport(User user)
        {
            _dbSet.Add(user);
            _dbContext.SaveChanges();
            return;
        }
        #endregion
    }
}
