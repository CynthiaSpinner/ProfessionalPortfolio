using Portfolio.Data.Repositories;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services;

public class SkillsCategoryService : ISkillsCategoryService
{
    private readonly ISkillsCategoryRepository _repository;

    public SkillsCategoryService(ISkillsCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SkillsCategory>> GetOrderedAsync() =>
        await _repository.GetAllOrderedAsync();

    public async Task<(bool Success, string Message, int? Id)> SaveCategoryAsync(int? id, string title, string? description, string? skillsText, int displayOrder, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(title))
            return (false, "Title is required.", null);

        var skillsList = ParseSkillsText(skillsText ?? "");

        if (id.HasValue && id.Value > 0)
        {
            var cat = await _repository.GetByIdAsync(id.Value);
            if (cat == null) return (false, "Category not found.", null);
            cat.Title = title.Trim();
            cat.Description = description?.Trim() ?? "";
            cat.Skills = skillsList;
            cat.DisplayOrder = displayOrder;
            cat.IsActive = isActive;
            cat.ImagePath = cat.ImagePath ?? "";
            await _repository.UpdateAsync(cat);
            return (true, "Skills category saved.", cat.Id);
        }

        var newCat = new SkillsCategory
        {
            Title = title.Trim(),
            Description = description?.Trim() ?? "",
            Skills = skillsList,
            DisplayOrder = displayOrder,
            IsActive = isActive,
            ImagePath = ""
        };
        newCat = await _repository.AddAsync(newCat);
        return (true, "Skills category saved.", newCat.Id);
    }

    public async Task<(bool Success, string Message)> DeleteCategoryAsync(int id)
    {
        var cat = await _repository.GetByIdAsync(id);
        if (cat == null) return (false, "Category not found.");
        await _repository.DeleteAsync(cat);
        return (true, "Category deleted.");
    }

    public async Task<(bool Success, string Message)> LoadDefaultCategoriesIfMissingAsync()
    {
        await _repository.LoadDefaultCategoriesIfMissingAsync();
        return (true, "Default skills categories loaded. Refresh the page to see them.");
    }

    private static List<string> ParseSkillsText(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return new List<string>();
        return text.Split(new[] { '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToList();
    }
}
