using Portfolio.Models.Portfolio;

namespace Portfolio.Services.Interfaces;

public interface ISiteSettingsService
{
    Task<SiteSettings> GetOrCreateAsync();
    Task<(bool Success, string Message)> SaveNavSettingsAsync(bool showGraphicDesignLink, bool showDesignLink);
    Task<(bool Success, string Message)> SaveSkillsLinkTextAsync(string? linkText);
}
