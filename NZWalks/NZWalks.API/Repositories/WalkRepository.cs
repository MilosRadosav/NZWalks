using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _context;
        private readonly IMapper _mapper;

        public WalkRepository(NZWalksDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _context.Walks
                .Include(x=> x.Region)
                .Include(x=> x.WalkDifficulty)
                .ToListAsync();
        }


        public async Task<Walk> GetAsync(Guid Id)
        {
            return await _context.Walks
                .Include(x=> x.Region)
                .Include(x=> x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }


        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return await _context.Walks.Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == walk.Id);
        }
        public async Task<Walk> DeleteAsync(Guid Id)
        {
            var walk = await _context.Walks.FirstOrDefaultAsync(x => x.Id == Id);

            if (walk == null)
            {
                return null;
            }

            //Delete region

            _context.Walks.Remove(walk);
            await _context.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid Id, Walk walk)
        {
            var existingWalk = await _context.Walks.FirstOrDefaultAsync(x => x.Id == Id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;

            await _context.SaveChangesAsync();

            return existingWalk;
        }
    }
}
