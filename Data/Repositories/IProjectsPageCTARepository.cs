using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public interface IProjectsPageCTARepository
    {
        Task<ProjectsPageCTA?> GetFirstOrDefaultAsync();
        Task<ProjectsPageCTA> AddAsync(ProjectsPageCTA entity);
        Task UpdateAsync(ProjectsPageCTA entity);
    }
}
