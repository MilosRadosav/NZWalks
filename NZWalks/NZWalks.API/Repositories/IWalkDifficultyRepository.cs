using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkDifficultyRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();
        Task<WalkDifficulty> GetAsync(Guid Id);
        Task<WalkDifficulty> AddAsync(WalkDifficulty walk);
        Task<WalkDifficulty> DeleteAsync(Guid Id);
        Task<WalkDifficulty> UpdateAsync(Guid Id, WalkDifficulty walkDifficulty);
    }
}
