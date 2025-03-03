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

namespace Mafia.Application.Servises
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly UserManager<User> _userManager;
        public UsersService(IUsersRepository repository, IJwtTokenProvider jwtTokenProvider, UserManager<User> userManager)
        {
            _repository = repository;
            _jwtTokenProvider = jwtTokenProvider;
            _userManager = userManager;
        }

        public async Task<User> Get(string email)
        {
            return await _repository.GetByEmail(email);
        }


        public async Task Register(string userName, string email, string password)
        {
            await _repository.Create(new User { UserName = userName, Email = email }, password);
        }

        public async Task<(string JwtToken, string RefreshToken)> Login(string email, string password)
        {
            User? user = await _repository.GetByEmail(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                throw new InvalidOperationException("invalid username or password") ;


            var JwtToken = _jwtTokenProvider.GenerateJwtToken(user, new List<string> { "user" });
            var RefreshToken = _jwtTokenProvider.GenerateRefreshToken();
            return (JwtToken, RefreshToken);
        }

        public async Task Update(string id, string userName, string email, string password)
        {
            await _repository.Upadate(id, userName, email, password);
        }

        public async Task DeleteByEmail(string email)
        {
            await _repository.DeleteByEmail(email);

        }


    }
}
