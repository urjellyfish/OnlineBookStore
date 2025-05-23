using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user);
        //Task<bool> Login(string username, string password);
        Task<bool> UserExistsAsync(string email);
        //Task<bool> ChangePassword(string username, string newPassword);
        //Task<bool> ResetPassword(string username, string newPassword);
        //Task<bool> DeleteUser(string username);
        //Task<bool> UpdateUserRole(string username, string newRole);
    }
}
