using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;

namespace Portfolio.Data.Repositories
{
    public class FeaturesSectionRepository : IFeaturesSectionRepository
    {
        private readonly PortfolioContext _context;

        public FeaturesSectionRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<FeaturesSection?> GetFirstOrDefaultAsync()
        {
            return await _context.FeaturesSections.FirstOrDefaultAsync();
        }

        public async Task<FeaturesSection> AddAsync(FeaturesSection section)
        {
            _context.FeaturesSections.Add(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task UpdateAsync(FeaturesSection section)
        {
            _context.Entry(section).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
