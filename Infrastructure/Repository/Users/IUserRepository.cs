using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Users
{
    public interface IUserRepository
    {
        Task UpdateUserAsync(User user);
        Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<User?> GetUserByIdAsync(Guid userId);
    }
}
