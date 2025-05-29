using Application.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Users
{
    public interface IUserService
    {
        Task<bool> UpdateUserAsync(Guid id, UserDto userDto);
        Task<User?> GetUserByIdAsync(Guid id);
    }
}
