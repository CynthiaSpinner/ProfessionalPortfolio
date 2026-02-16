using Portfolio.Models.Portfolio;

namespace Portfolio.Data.Repositories
{
    public interface ICTASectionRepository
    {
        Task<CTASection?> GetFirstOrDefaultAsync();
        Task<CTASection> AddAsync(CTASection section);
        Task UpdateAsync(CTASection section);
        Task<int> SaveChangesAsync();
    }
}
