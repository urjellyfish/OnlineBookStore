using Application.Dto;
using Application.Dto.LoginDto;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Login(string email, string password)
        {
            var user = await _authRepository.Login(email, password);
            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }
            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = CreateToken(user)
            };

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

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FName),
                new Claim(ClaimTypes.Surname, user.LName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // Use a secure key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Use a secure issuer
                audience: _configuration["Jwt:Audience"], // Use a secure audience
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Set token expiration
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}