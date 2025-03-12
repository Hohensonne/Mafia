using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mafia.Persistence.Repositories
{
    public class GameRegistrationRepository : IGameRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameRegistration>> GetAllAsync()
        {
            return await _context.GameRegistrations
                .Include(gr => gr.Game)
                .Include(gr => gr.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameRegistration>> GetAllByGameIdAsync(Guid gameId)
        {
            return await _context.GameRegistrations
                .Include(gr => gr.User)
                .Where(gr => gr.GameId == gameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameRegistration>> GetAllByUserIdAsync(string userId)
        {
            return await _context.GameRegistrations
                .Include(gr => gr.Game)
                .Where(gr => gr.UserId == userId)
                .ToListAsync();
        }

        public async Task<GameRegistration?> GetByIdAsync(Guid id)
        {
            return await _context.GameRegistrations
                .Include(gr => gr.Game)
                .Include(gr => gr.User)
                .FirstOrDefaultAsync(gr => gr.Id == id);
        }

        public async Task<GameRegistration?> GetByGameIdAndUserIdAsync(Guid gameId, string userId)
        {
            return await _context.GameRegistrations
                .FirstOrDefaultAsync(gr => gr.GameId == gameId && gr.UserId == userId);
        }

        public async Task<Guid> CreateAsync(GameRegistration registration)
        {
            await _context.GameRegistrations.AddAsync(registration);
            await _context.SaveChangesAsync();
            return registration.Id;
        }

        public async Task UpdateAsync(GameRegistration registration)
        {
            _context.GameRegistrations.Update(registration);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var registration = await _context.GameRegistrations.FindAsync(id);
            if (registration != null)
            {
                _context.GameRegistrations.Remove(registration);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ApproveAsync(Guid id)
        {
            var registration = await _context.GameRegistrations.FindAsync(id);
            if (registration != null)
            {
                registration.IsApproved = true;
                _context.GameRegistrations.Update(registration);
                await _context.SaveChangesAsync();
            }
        }
    }
} 