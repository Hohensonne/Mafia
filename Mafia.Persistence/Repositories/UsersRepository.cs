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
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IList<User>> GetAll()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> Create(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> Update(string id, string firstName, string lastName, string email, string phoneNumber, string password)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return IdentityResult.Failed();
            if (firstName != null)
                user.FirstName = firstName;
            if (lastName != null)
                user.LastName = lastName;
            if (email != null)
                user.Email = email;
            if (phoneNumber != null)
                user.PhoneNumber = phoneNumber;
            if (password != null)
                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteByEmail(string email)
        {
            User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
                return await _userManager.DeleteAsync(user);
            return IdentityResult.Failed();
        }
        
        public async Task<IdentityResult> UpdateProfileImage(string id, string profileImageUrl  )
        {
            var user = await _userManager.FindByIdAsync(id);
            user.ProfileImageUrl = profileImageUrl;
            return await _userManager.UpdateAsync(user);
        }
    }
}
