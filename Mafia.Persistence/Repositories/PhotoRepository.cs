using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mafia.Persistence.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public PhotoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Photo>> GetAllAsync()
        {
            return await _context.Photos.ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetAllByGameIdAsync(string gameId)
        {
            return await _context.Photos
                .Where(p => p.GameId == gameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Photos
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Photo?> GetByIdAsync(string id)
        {
            return await _context.Photos.FindAsync(id);
        }

        public async Task<string> CreateAsync(Photo photo)
        {
            await _context.Photos.AddAsync(photo);
            await _context.SaveChangesAsync();
            return photo.Id;
        }

        public async Task UpdateAsync(Photo photo)
        {
            _context.Photos.Update(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();
            }
        }
    }
} 