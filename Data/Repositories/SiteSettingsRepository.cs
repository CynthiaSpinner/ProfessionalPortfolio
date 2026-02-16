using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public class SiteSettingsRepository : ISiteSettingsRepository
    {
        private readonly PortfolioContext _context;

        public SiteSettingsRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<SiteSettings?> GetFirstOrDefaultAsync()
        {
            return await _context.SiteSettings.FirstOrDefaultAsync();
        }

        public async Task<SiteSettings> AddAsync(SiteSettings settings)
        {
            _context.SiteSettings.Add(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task UpdateAsync(SiteSettings settings)
        {
            settings.UpdatedAt = DateTime.UtcNow;
            _context.Entry(settings).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
