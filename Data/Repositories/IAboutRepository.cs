using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public interface IAboutRepository
    {
        Task<About?> GetFirstOrDefaultAsync();
    }
}
