using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
    }
}
