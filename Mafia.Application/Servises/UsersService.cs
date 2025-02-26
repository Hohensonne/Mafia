using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mafia.Logic.Models;
using Mafia.Persistence.Repositories;

namespace Mafia.Application.Servises
{
    public class UsersService
    {
        private readonly UsersRepository _repository;
        public UsersService(UsersRepository repository)
        {
            _repository = repository;
        }


        public async Task Register(string UserName, string Email, string)
        {
            await _repository.Create(user);
        }
    }
}
