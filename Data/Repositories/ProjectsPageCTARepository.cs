using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public class ProjectsPageCTARepository : IProjectsPageCTARepository
    {
        private readonly PortfolioContext _context;

        public ProjectsPageCTARepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<ProjectsPageCTA?> GetFirstOrDefaultAsync()
        {
            return await _context.ProjectsPageCTAs.FirstOrDefaultAsync();
        }

        public async Task<ProjectsPageCTA> AddAsync(ProjectsPageCTA entity)
        {
            _context.ProjectsPageCTAs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ProjectsPageCTA entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
