using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public class AboutRepository : IAboutRepository
    {
        private readonly PortfolioContext _context;

        public AboutRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<About?> GetFirstOrDefaultAsync()
        {
            return await _context.Abouts.FirstOrDefaultAsync();
        }
    }
}
