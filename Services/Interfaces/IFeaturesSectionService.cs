using Portfolio.Services.DTOs;

namespace Portfolio.Services.Interfaces;

public interface IFeaturesSectionService
{
    /// <summary>Gets the current section or null; maps to admin DTO shape.</summary>
    Task<object?> GetAdminDtoAsync();

    /// <summary>Gets the section for the public API (sectionTitle, sectionSubtitle, features array, lastModified).</summary>
    Task<object> GetForPublicApiAsync();

    /// <summary>Creates or updates the section from the admin form.</summary>
    Task<(bool Success, string Message)> SaveFromDtoAsync(FeaturesSectionEditDto dto);
}
