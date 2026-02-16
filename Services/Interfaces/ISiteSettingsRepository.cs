using Portfolio.Models.Portfolio;

namespace Portfolio.Services.Interfaces
{
    public interface ISiteSettingsRepository
    {
        Task<SiteSettings?> GetFirstOrDefaultAsync();
        Task<SiteSettings> AddAsync(SiteSettings settings);
        Task UpdateAsync(SiteSettings settings);
        Task<int> SaveChangesAsync();
    }
}
