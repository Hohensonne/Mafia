using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mafia.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games
                .Include(g => g.Location)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetAllByLocationIdAsync(Guid locationId)
        {
            return await _context.Games
                .Where(g => g.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetUpcomingGamesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Games
                .Include(g => g.Location)
                .Where(g => g.StartTime > now && g.EndOfRegistration > now)
                .OrderBy(g => g.StartTime)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(Guid id)
        {
            return await _context.Games
                .Include(g => g.Location)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game?> GetByIdWithRegistrationsAsync(Guid id)
        {
            return await _context.Games
                .Include(g => g.Location)
                .Include(g => g.GameRegistrations)
                    .ThenInclude(gr => gr.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game?> GetByIdWithPhotosAsync(Guid id)
        {
            return await _context.Games
                .Include(g => g.Location)
                .Include(g => g.Photos)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Guid> CreateAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game.Id;
        }

        public async Task UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IncrementCurrentPlayersAsync(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null || game.CurrentPlayers >= game.MaxPlayers)
            {
                return false;
            }

            game.CurrentPlayers++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DecrementCurrentPlayersAsync(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null || game.CurrentPlayers <= 0)
            {
                return false;
            }

            game.CurrentPlayers--;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 