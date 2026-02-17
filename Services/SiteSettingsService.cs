using Portfolio.Data.Repositories;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services;

public class SiteSettingsService : ISiteSettingsService
{
    private readonly ISiteSettingsRepository _siteSettingsRepository;
    private readonly ISkillsCategoryRepository _skillsCategoryRepository;

    public SiteSettingsService(
        ISiteSettingsRepository siteSettingsRepository,
        ISkillsCategoryRepository skillsCategoryRepository)
    {
        _siteSettingsRepository = siteSettingsRepository;
        _skillsCategoryRepository = skillsCategoryRepository;
    }

    public async Task<SiteSettings> GetOrCreateAsync()
    {
        var settings = await _siteSettingsRepository.GetFirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new SiteSettings();
            await _siteSettingsRepository.AddAsync(settings);
        }
        return settings;
    }

    public async Task<(bool Success, string Message)> SaveNavSettingsAsync(bool showGraphicDesignLink, bool showDesignLink)
    {
        var settings = await GetOrCreateAsync();
        settings.ShowGraphicDesignLink = showGraphicDesignLink;
        settings.ShowDesignLink = showDesignLink;
        settings.UpdatedAt = DateTime.UtcNow;
        await _siteSettingsRepository.UpdateAsync(settings);
        return (true, "Navigation settings saved.");
    }

    public async Task<(bool Success, string Message)> SaveSkillsLinkTextAsync(string? linkText)
    {
        var categories = await _skillsCategoryRepository.GetAllOrderedAsync();
        var first = categories.FirstOrDefault();
        if (first == null)
            return (false, "Add at least one skills category first.");
        first.TeaserLinkText = string.IsNullOrWhiteSpace(linkText) ? null : linkText.Trim();
        await _skillsCategoryRepository.UpdateAsync(first);
        return (true, "Skills teaser link text saved.");
    }
}
