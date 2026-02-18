using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public class ProjectsPageHeroRepository : IProjectsPageHeroRepository
    {
        private readonly PortfolioContext _context;

        public ProjectsPageHeroRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<ProjectsPageHero?> GetFirstOrDefaultAsync()
        {
            return await _context.ProjectsPageHeroes.FirstOrDefaultAsync();
        }

        public async Task<ProjectsPageHero> AddAsync(ProjectsPageHero entity)
        {
            _context.ProjectsPageHeroes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ProjectsPageHero entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
