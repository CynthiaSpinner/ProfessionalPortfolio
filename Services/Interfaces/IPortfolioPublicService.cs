using Portfolio.Services.DTOs;

namespace Portfolio.Services.Interfaces;

/// <summary>Provides data for the public portfolio API (home page, etc.).</summary>
public interface IPortfolioPublicService
{
    Task<SkillsPublicDto> GetSkillsForPublicAsync();
}
