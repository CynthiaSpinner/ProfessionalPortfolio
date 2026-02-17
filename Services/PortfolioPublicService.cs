using Portfolio.Data.Repositories;
using Portfolio.Models.Portfolio;
using Portfolio.Services.DTOs;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services;

public class PortfolioPublicService : IPortfolioPublicService
{
    private readonly ISkillsCategoryRepository _skillsCategoryRepository;
    private readonly IAboutRepository _aboutRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ISiteSettingsRepository _siteSettingsRepository;

    public PortfolioPublicService(
        ISkillsCategoryRepository skillsCategoryRepository,
        IAboutRepository aboutRepository,
        IProjectRepository projectRepository,
        ISiteSettingsRepository siteSettingsRepository)
    {
        _skillsCategoryRepository = skillsCategoryRepository;
        _aboutRepository = aboutRepository;
        _projectRepository = projectRepository;
        _siteSettingsRepository = siteSettingsRepository;
    }

    public async Task<SkillsPublicDto> GetSkillsForPublicAsync()
    {
        var all = await _skillsCategoryRepository.GetAllOrderedAsync();
        var categories = all.Where(c => c.IsActive).ToList();
        var linkText = all.FirstOrDefault()?.TeaserLinkText?.Trim();
        if (string.IsNullOrEmpty(linkText)) linkText = "View what I spin?";
        return new SkillsPublicDto { Categories = categories, LinkText = linkText };
    }

    public async Task<About?> GetAboutAsync() => await _aboutRepository.GetFirstOrDefaultAsync();

    public async Task<List<Project>> GetProjectsAsync() => await _projectRepository.GetAllAsync();

    public async Task<NavSettingsDto> GetNavSettingsAsync()
    {
        var settings = await _siteSettingsRepository.GetFirstOrDefaultAsync();
        if (settings == null)
            return new NavSettingsDto(true, true);
        return new NavSettingsDto(settings.ShowGraphicDesignLink, settings.ShowDesignLink);
    }

    public async Task<List<SkillsCategory>> GetSkillsCategoriesOrderedAsync() =>
        await _skillsCategoryRepository.GetAllOrderedAsync();
}
