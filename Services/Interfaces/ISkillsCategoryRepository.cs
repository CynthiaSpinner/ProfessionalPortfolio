using Portfolio.Models.Portfolio;

namespace Portfolio.Services.Interfaces
{
    public interface ISkillsCategoryRepository
    {
        Task<List<SkillsCategory>> GetAllOrderedAsync();
        Task<SkillsCategory?> GetByIdAsync(int id);
        Task<SkillsCategory?> GetByTitleAsync(string title);
        Task<SkillsCategory> AddAsync(SkillsCategory category);
        Task UpdateAsync(SkillsCategory category);
        Task DeleteAsync(SkillsCategory category);
        Task<int> SaveChangesAsync();
        /// <summary>Adds default categories (Frontend, Backend, Databases, Tools, Practices) if they do not exist.</summary>
        Task LoadDefaultCategoriesIfMissingAsync();
    }
}
