using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;
using Mafia.Core.Interfaces;

namespace Mafia.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public UsersRepository(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<User?> GetByEmail(string email)
        {
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<IList<User>> GetAll()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task Create(User user, string password)
        {
            await _userManager.CreateAsync(user, password);
        }

        public async Task Upadate(string id, string userName, string email, string password)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = userName;
            user.Email = email;
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteByEmail(string email)
        {
            User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }
    }


}
