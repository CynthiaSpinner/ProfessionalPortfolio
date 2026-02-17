using Portfolio.Data.Repositories;
using Portfolio.Services.DTOs;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services;

public class PortfolioPublicService : IPortfolioPublicService
{
    private readonly ISkillsCategoryRepository _skillsCategoryRepository;

    public PortfolioPublicService(ISkillsCategoryRepository skillsCategoryRepository)
    {
        _skillsCategoryRepository = skillsCategoryRepository;
    }

    public async Task<SkillsPublicDto> GetSkillsForPublicAsync()
    {
        var all = await _skillsCategoryRepository.GetAllOrderedAsync();
        var categories = all.Where(c => c.IsActive).ToList();
        var linkText = all.FirstOrDefault()?.TeaserLinkText?.Trim();
        if (string.IsNullOrEmpty(linkText)) linkText = "View what I spin?";
        return new SkillsPublicDto { Categories = categories, LinkText = linkText };
    }
}
