using Application.Dto;
using Domain.Entities;
using Infrastructure.Repository.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null)
            {
               return false;
            }
            user.Email = userDto.Email;
            user.FName = userDto.FName;
            user.LName = userDto.LName;
            user.Dob = userDto.Dob;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }
}
