using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mafia.Core.Models;
using Mafia.Persistence.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Mafia.Infrastructre;
using Mafia.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;

namespace Mafia.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly UserManager<User> _userManager;
        private readonly IFileRepository _fileRepository;
        public UsersService(IUsersRepository repository, IJwtTokenProvider jwtTokenProvider, UserManager<User> userManager, IFileRepository fileRepository)
        {
            _repository = repository;
            _jwtTokenProvider = jwtTokenProvider;
            _userManager = userManager;
            _fileRepository = fileRepository;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _repository.GetByEmail(email);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _repository.GetAll();
        }


        public async Task Register(string FirstName, string LastName, string email, string phoneNumber, string password, IFormFile profileImage)
        {
            if (!Regex.IsMatch(password, @"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$"))
                throw new AuthenticationFailureException("invalid phone number");

            var userId = Guid.NewGuid().ToString();
            string imageUrl = "";
            if(profileImage != null)
            {
                imageUrl = await _fileRepository.SaveProfileImageAsync(userId, profileImage);
            }
            var user = new User { 
                Id = userId,
                FirstName = FirstName,
                LastName = LastName,
                Email = email,
                PhoneNumber = phoneNumber,
                RegistrationDate = DateTime.UtcNow,
                ProfileImageUrl = imageUrl
            };
            var result = await _repository.Create(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AuthenticationFailureException($"user not created: {errors}");
            }
            await _userManager.AddToRoleAsync(user, "User");          



            //var user = new User { 
            //    UserName = userName, 
            //    Email = email, 
            //    RegistrationDate = DateTime.UtcNow 
            //    };
            //var result = await _repository.Create(user, password);
            //if (!result.Succeeded)
            //{
            //    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            //    throw new AuthenticationFailureException($"user not created: {errors}");
            //}
            //if (profileImage != null)
            //{
            //    var imageUrl = await _fileRepository.SaveProfileImageAsync(user.Id, profileImage);
            //    user.ProfileImageUrl = imageUrl;
            //    await _repository.UpdateProfileImage(user.Id, imageUrl);
            //}
            //
            //
            //await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<(string jwtToken, string refreshToken)> Login(string email, string password)
        {
            User? user = await _repository.GetByEmail(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                throw new AuthenticationFailureException("invalid username or password") ;


            var JwtToken = _jwtTokenProvider.GenerateJwtToken(user, _userManager.GetRolesAsync(user).Result);
            var RefreshToken = _jwtTokenProvider.GenerateRefreshToken();
            
            // Сохраняем refresh token в базу данных
            user.RefreshToken = RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _jwtTokenProvider.SaveRefreshTokenAsync(user);
            
            return (JwtToken, RefreshToken);
        }

        public async Task Update(string id, string firstName, string lastName, string email, string phoneNumber, string password)
        {            
            if (phoneNumber != null && !Regex.IsMatch(phoneNumber, @"^\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$"))
                throw new AuthenticationFailureException("invalid phone number");

            var result = await _repository.Update(id, firstName, lastName, email, phoneNumber, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AuthenticationFailureException($"user not updated: {errors}");
            }
        }

        public async Task DeleteByEmail(string email)
        {
            var result = await _repository.DeleteByEmail(email);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new AuthenticationFailureException($"user not deleted: {errors}");
            }
        }

        public async Task UpdateProfileImage(string email, IFormFile profileImage)
        {
            var user = await _repository.GetByEmail(email);
            if (user == null)
                throw new AuthenticationFailureException("User not found");
            await _fileRepository.DeleteProfileImageAsync(user.ProfileImageUrl);
            var imageUrl = await _fileRepository.SaveProfileImageAsync(user.Id, profileImage);
            await _repository.UpdateProfileImage(user.Id, imageUrl);
        }

        public async Task<(string jwtToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _jwtTokenProvider.GetRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                throw new AuthenticationFailureException("Invalid refresh token. User not found");
            }

            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                throw new AuthenticationFailureException("Token expired");
            }
            
            
            var newJwtToken = _jwtTokenProvider.GenerateJwtToken(user, _userManager.GetRolesAsync(user).Result);
            var newRefreshToken = _jwtTokenProvider.GenerateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _jwtTokenProvider.SaveRefreshTokenAsync(user);
            
            return (newJwtToken, newRefreshToken);
        }

    }
}
