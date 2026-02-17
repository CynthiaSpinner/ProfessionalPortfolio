using Portfolio.Models.Portfolio;
using Portfolio.Services.DTOs;

namespace Portfolio.Services.Interfaces;

/// <summary>Provides data for the public portfolio API (home page, etc.).</summary>
public interface IPortfolioPublicService
{
    Task<SkillsPublicDto> GetSkillsForPublicAsync();
    Task<About?> GetAboutAsync();
    Task<List<Project>> GetProjectsAsync();
    Task<NavSettingsDto> GetNavSettingsAsync();
    /// <summary>All skills categories in display order (e.g. for admin/MVC views).</summary>
    Task<List<SkillsCategory>> GetSkillsCategoriesOrderedAsync();
}

public record NavSettingsDto(bool ShowGraphicDesignLink, bool ShowDesignLink);
