using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mafia.Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Mafia.Persistence.Repositories
{
    public class UsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public UsersRepository(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<Guid> Create(User user)
        {
            await _userManager.CreateAsync(new User { }, user.pas
            return 
        }
    }


}
