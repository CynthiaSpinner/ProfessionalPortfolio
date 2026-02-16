using Portfolio.Models.Portfolio;

namespace Portfolio.Services.Interfaces
{
    public interface IFeaturesSectionRepository
    {
        Task<FeaturesSection?> GetFirstOrDefaultAsync();
        Task<FeaturesSection> AddAsync(FeaturesSection section);
        Task UpdateAsync(FeaturesSection section);
        Task<int> SaveChangesAsync();
    }
}
