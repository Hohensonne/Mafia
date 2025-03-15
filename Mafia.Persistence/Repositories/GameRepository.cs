using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
                .ToListAsync();
        }


        public async Task<IEnumerable<Game>> GetUpcomingGamesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Games
                .Where(g => g.StartTime > now && g.EndOfRegistration > now)
                .OrderBy(g => g.StartTime)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(string id)
        {
            return await _context.Games
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game>> GetRegisteredGamesAsync(string userId)
        {
            return await _context.GameRegistrations
                .Where(gr => gr.UserId == userId)
                .Select(gr => gr.Game)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdWithPhotosAsync(string id)
        {
            return await _context.Games
                .Include(g => g.Photos)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<string> CreateAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game.Id;
        }

        public async Task<string> UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return game.Id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            return id;
        }

        public async Task IncrementPlayersAsync(string id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null || game.CurrentPlayers >= game.MaxPlayers)
            {
                throw new Exception("Game is full");
            }

            game.CurrentPlayers++;
            await _context.SaveChangesAsync();
        }

        public async Task DecrementPlayersAsync(string id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null || game.CurrentPlayers <= 0)
            {
                throw new Exception("Game has no players");
            }

            game.CurrentPlayers--;
            await _context.SaveChangesAsync();
        }
    }
} 