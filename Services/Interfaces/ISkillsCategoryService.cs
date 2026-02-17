using Portfolio.Models.Portfolio;

namespace Portfolio.Services.Interfaces;

public interface ISkillsCategoryService
{
    Task<List<SkillsCategory>> GetOrderedAsync();
    Task<(bool Success, string Message, int? Id)> SaveCategoryAsync(int? id, string title, string? description, string? skillsText, int displayOrder, bool isActive);
    Task<(bool Success, string Message)> DeleteCategoryAsync(int id);
    Task<(bool Success, string Message)> LoadDefaultCategoriesIfMissingAsync();
}
