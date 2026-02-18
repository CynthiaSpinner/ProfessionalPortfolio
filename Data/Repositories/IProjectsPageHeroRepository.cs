using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public interface IProjectsPageHeroRepository
    {
        Task<ProjectsPageHero?> GetFirstOrDefaultAsync();
        Task<ProjectsPageHero> AddAsync(ProjectsPageHero entity);
        Task UpdateAsync(ProjectsPageHero entity);
    }
}
