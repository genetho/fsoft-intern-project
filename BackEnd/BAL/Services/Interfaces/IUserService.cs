using BAL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DAL.Entities.User;

namespace BAL.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> SendOtp(string email);
        Task<bool> ResetPassword(string newPassword, string Otp);
        Task<AccountViewModel> Login(string email, string password);
        Task<UserViewModel> Add(UserAccountViewModel user);
        Task<UserViewModel> Edit(UserViewModel user);
        Task<UpLoadExcelFileResponse> ImportUser(UpLoadExcelFileRequest request, string path);
        Task<bool> DeActivate(long id);
        Task<UserViewModel> ChangleRole(long id, long IdRole);
        Task<bool> Delete(long id);
        List<UserViewModel> GetAll(string? keyword, List<string>? sortby, int PAGE_SIZE, int PAGE_NUMBER);
        Task<User> GetById(long id);
        Task<bool> CheckEdit(long userId);

        #region Other groups
        User GetByID(long id);
        Task<IEnumerable<TrainerViewModel>> GetTrainers();
        #region Group 5 - Authentication & Authorization
        public User GetUser(string username);
        Task<User> GetUser(string username, string password);
        #endregion
        UserViewModel GetUserViewModelById(long userId);
        #endregion

        #region Functions
        void Save();
        void SaveAsync();
        #endregion
    }
}
