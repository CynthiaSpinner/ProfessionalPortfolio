using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public interface IFeaturesSectionRepository
    {
        Task<FeaturesSection?> GetFirstOrDefaultAsync();
        Task<FeaturesSection> AddAsync(FeaturesSection section);
        Task UpdateAsync(FeaturesSection section);
        Task<int> SaveChangesAsync();
    }
}
