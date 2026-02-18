namespace Portfolio.Services.Interfaces
{
    public interface IProjectsPageService
    {
        Task<object?> GetHeroForPublicApiAsync();
        Task<object?> GetHeroForAdminAsync();
        Task<(bool Success, string Message)> SaveHeroAsync(string title, string? subtitle, string? buttonText, string? buttonUrl);

        Task<object> GetCTAForPublicApiAsync();
        Task<object?> GetCTAForAdminAsync();
        Task<(bool Success, string Message)> SaveCTAAsync(string title, string subtitle, string buttonText, string buttonLink);
    }
}
