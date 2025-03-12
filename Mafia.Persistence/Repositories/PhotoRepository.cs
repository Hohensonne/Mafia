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
            return await _context.Photos
                .Include(p => p.Game)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetAllByGameIdAsync(Guid gameId)
        {
            return await _context.Photos
                .Include(p => p.User)
                .Where(p => p.GameId == gameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Photos
                .Include(p => p.Game)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Photo?> GetByIdAsync(Guid id)
        {
            return await _context.Photos
                .Include(p => p.Game)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> CreateAsync(Photo photo)
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

        public async Task DeleteAsync(Guid id)
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