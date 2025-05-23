using Application.Dto;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<User?> RegisterAsync(UserDto userDto)
        {
            if (await _authRepository.UserExistsAsync(userDto.Email))
            {
                return null; // User already exists
            }

            var user = new User
            {
                Email = userDto.Email,
                //PasswordHash = userDto.Password,
                FName = userDto.FName,
                LName = userDto.LName,
                Dob = userDto.Dob,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var passwordHash = new PasswordHasher<User>();
            user.PasswordHash = passwordHash.HashPassword(user, userDto.Password);

            await _authRepository.RegisterAsync(user);
            return user; // Registration successful
        }
    }
}
