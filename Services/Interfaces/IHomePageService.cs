using Microsoft.AspNetCore.Http;
using Portfolio.Models.Portfolio;

namespace Portfolio.Services.Interfaces
{
    public interface IHomePageService
    {
        // General CRUD Operations
        Task<HomePage?> GetHomePageAsync();
        Task<HomePage> CreateHomePageAsync(HomePage homePage);
        Task<HomePage> UpdateHomePageAsync(HomePage homePage);
        Task<bool> DeleteHomePageAsync(int id);

        // Header Section Operations
        Task<HomePage> UpdateHeaderSectionAsync(HomePage homePage);
        Task<string> UploadHeaderBackgroundImageAsync(IFormFile file);
        Task<string> UploadHeaderBackgroundVideoAsync(IFormFile file);

        // Skills Section Operations
        Task<HomePage> UpdateSkillsSectionAsync(HomePage homePage);
        Task<HomePage> AddSkillsCategoryAsync(int homePageId, SkillsCategory category);
        Task<HomePage> UpdateSkillsCategoryAsync(int homePageId, SkillsCategory category);
        Task<HomePage> RemoveSkillsCategoryAsync(int homePageId, string categoryTitle);
        Task<string> UploadSkillCategoryImageAsync(IFormFile file);

        // Featured Projects Section Operations
        Task<HomePage> UpdateFeaturedProjectsSectionAsync(HomePage homePage);
        Task<HomePage> AddFeaturedProjectAsync(int homePageId, int projectId);
        Task<HomePage> RemoveFeaturedProjectAsync(int homePageId, int projectId);
        Task<HomePage> ReorderFeaturedProjectsAsync(int homePageId, List<int> projectIds);

        // CTA Section Operations
        Task<HomePage> UpdateCTASectionAsync(HomePage homePage);
        Task<string> UploadCTABackgroundImageAsync(IFormFile file);

        // General Settings
        Task<HomePage> UpdateGeneralSettingsAsync(HomePage homePage);
        Task<bool> ToggleHomePageStatusAsync(int id);
        Task<HomePage> UpdateDisplayOrderAsync(int id, int newOrder);
    }
}