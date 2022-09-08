using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _context;

        public WalkDifficultyRepository(NZWalksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _context.WalkDifficulty.ToListAsync();
        }
        public async Task<WalkDifficulty> GetAsync(Guid Id)
        {
            return await _context.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid Id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await _context.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == Id);

            if (existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = walkDifficulty.Code;

            await _context.SaveChangesAsync();

            return existingWalkDifficulty;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id =  Guid.NewGuid();
            await _context.AddAsync(walkDifficulty);
            await _context.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid Id)
        {
            var existingWalkDifficulty = _context.WalkDifficulty.FirstOrDefault(x => x.Id == Id);

            if (existingWalkDifficulty==null)
            {
                return null;
            }

             _context.WalkDifficulty.Remove(existingWalkDifficulty);
            await _context.SaveChangesAsync();

            return existingWalkDifficulty;
        }

   

        
    }
}
