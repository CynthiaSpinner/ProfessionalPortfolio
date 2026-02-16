using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;

namespace Portfolio.Data.Repositories
{
    public class CTASectionRepository : ICTASectionRepository
    {
        private readonly PortfolioContext _context;

        public CTASectionRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<CTASection?> GetFirstOrDefaultAsync()
        {
            return await _context.CTASections.FirstOrDefaultAsync();
        }

        public async Task<CTASection> AddAsync(CTASection section)
        {
            _context.CTASections.Add(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task UpdateAsync(CTASection section)
        {
            section.UpdatedAt = DateTime.UtcNow;
            _context.Entry(section).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
