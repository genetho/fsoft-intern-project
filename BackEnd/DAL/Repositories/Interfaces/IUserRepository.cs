using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> SendOtp(string email);
        Task<bool> ResetPassword(string newPassword, string Otp);
        Task<User> Login(string email, string password);
        IQueryable<User> GetAllUser(string? keyword, List<string>? sortby, int PAGE_SIZE, int PAGE_NUMBER);
        Task<bool> Delete(long id);
        Task<User> GetById(long id);
        Task<User> Edit(User user);
        Task<User> Add(User user);
        Task<bool> DeActivate(long id);
        Task<User> ChangleRole(long id, long IdRole);
        Task<bool> CheckEdit(long userId);
        User GetUserById(long UserId);
        #region Other groups
        User Get(long id);
        Task<IEnumerable<User>> GetTrainers();
        #region Group 5 - Authentication & Authorization
        public User GetUser(string username);
        Task<User> GetUser(string username, string password);
        Task<User> GetUserAsync(long userId);
        #endregion
        // Team6
        User GetUser(long id);
        List<User> GetUsers(long id);
        // Team6
        //team4
        List<User> GetUsersForImport();
        void CreateUserForImport(User user);
        #endregion        
    }
}
