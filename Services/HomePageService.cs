using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services.DTOs;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services
{
    public class HomePageService : IHomePageService
    {
        private readonly PortfolioContext _context;
        private readonly IFileService _fileService;
        private HomePage? _cachedHomePage;
        private DateTime _lastCacheTime = DateTime.MinValue;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5); // 5 minute cache since no polling

        public HomePageService(PortfolioContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<HeroPublicDto> GetHeroForPublicApiAsync(string baseUrl)
        {
            var homePage = await GetHomePageAsync();
            if (homePage == null)
            {
                return new HeroPublicDto
                {
                    Title = "Welcome to My Portfolio",
                    Subtitle = "I am a passionate software engineer specializing in full-stack development, with expertise in creating modern, scalable applications.",
                    Description = "",
                    BackgroundImageUrl = "",
                    BackgroundVideoUrl = null,
                    PrimaryButtonText = "View Projects",
                    PrimaryButtonUrl = "/projects",
                    OverlayColor = "#000000",
                    OverlayOpacity = 0.5f,
                    LastModified = DateTime.UtcNow
                };
            }
            var version = (homePage.UpdatedAt ?? homePage.CreatedAt).Ticks;
            var backgroundImageUrl = homePage.HeaderBackgroundImageData != null
                ? $"{baseUrl}/api/portfolio/hero-image?v={version}"
                : (homePage.HeaderBackgroundImageUrl ?? "");
            return new HeroPublicDto
            {
                Title = homePage.HeaderTitle ?? "Welcome to My Portfolio",
                Subtitle = homePage.HeaderSubtitle ?? "",
                Description = homePage.HeaderDescription ?? "",
                BackgroundImageUrl = backgroundImageUrl,
                BackgroundVideoUrl = homePage.HeaderBackgroundVideoUrl,
                PrimaryButtonText = homePage.HeaderPrimaryButtonText ?? "View Projects",
                PrimaryButtonUrl = homePage.HeaderPrimaryButtonUrl ?? "/projects",
                OverlayColor = homePage.HeaderOverlayColor ?? "#000000",
                OverlayOpacity = homePage.HeaderOverlayOpacity ?? 0.5f,
                LastModified = homePage.UpdatedAt ?? homePage.CreatedAt
            };
        }

        public async Task<HomePage?> GetHomePageAsync()
        {
            // Check if cache is still valid
            if (_cachedHomePage != null && DateTime.UtcNow - _lastCacheTime < _cacheExpiration)
            {
                return _cachedHomePage;
            }

            // Fetch from database and update cache
            _cachedHomePage = await _context.HomePages
                .FirstOrDefaultAsync(hp => hp.IsActive);
            _lastCacheTime = DateTime.UtcNow;
            
            return _cachedHomePage;
        }

        // Method to invalidate cache when data is updated
        private void InvalidateCache()
        {
            _cachedHomePage = null;
            _lastCacheTime = DateTime.MinValue;
        }

        // Method to force cache refresh
        public void ForceCacheRefresh()
        {
            InvalidateCache();
        }

        public async Task<HomePage> CreateHomePageAsync(HomePage homePage)
        {
            _context.HomePages.Add(homePage);
            await _context.SaveChangesAsync();
            return homePage;
        }

        public async Task<HomePage> UpdateHomePageAsync(HomePage homePage)
        {
            homePage.UpdatedAt = DateTime.UtcNow;
            _context.Entry(homePage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            // Invalidate cache after update
            InvalidateCache();
            
            return homePage;
        }

        public async Task<bool> DeleteHomePageAsync(int id)
        {
            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
                return false;

            _context.HomePages.Remove(homePage);
            await _context.SaveChangesAsync();
            return true;
        }

        // Header Section Operations
        public async Task<HomePage> UpdateHeaderSectionAsync(HomePage homePage)
        {
            var existingPage = await _context.HomePages.FindAsync(homePage.Id);
            if (existingPage == null)
                return await CreateHomePageAsync(homePage);

            // Update only header-related properties
            existingPage.HeaderTitle = homePage.HeaderTitle;
            existingPage.HeaderSubtitle = homePage.HeaderSubtitle;
            existingPage.HeaderDescription = homePage.HeaderDescription;
            existingPage.HeaderBackgroundImageUrl = homePage.HeaderBackgroundImageUrl;
            existingPage.HeaderBackgroundVideoUrl = homePage.HeaderBackgroundVideoUrl;
            existingPage.HeaderBackgroundStyle = homePage.HeaderBackgroundStyle;
            existingPage.HeaderPrimaryButtonText = homePage.HeaderPrimaryButtonText;
            existingPage.HeaderPrimaryButtonUrl = homePage.HeaderPrimaryButtonUrl;
            existingPage.HeaderSecondaryButtonText = homePage.HeaderSecondaryButtonText;
            existingPage.HeaderSecondaryButtonUrl = homePage.HeaderSecondaryButtonUrl;
            existingPage.ShowHeaderPrimaryButton = homePage.ShowHeaderPrimaryButton;
            existingPage.ShowHeaderSecondaryButton = homePage.ShowHeaderSecondaryButton;
            existingPage.HeaderOverlayColor = homePage.HeaderOverlayColor;
            existingPage.HeaderOverlayOpacity = homePage.HeaderOverlayOpacity;
            existingPage.HeaderTextColor = homePage.HeaderTextColor;
            existingPage.HeaderButtonColor = homePage.HeaderButtonColor;
            existingPage.HeaderButtonTextColor = homePage.HeaderButtonTextColor;
            existingPage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingPage;
        }

        public async Task<string> UploadHeaderBackgroundImageAsync(IFormFile file)
        {
            return await _fileService.UploadFileAsync(file, "homepage/header");
        }

        public async Task<string> UploadHeaderBackgroundVideoAsync(IFormFile file)
        {
            return await _fileService.UploadFileAsync(file, "homepage/header");
        }

        // Skills Section Operations
        public async Task<HomePage> UpdateSkillsSectionAsync(HomePage homePage)
        {
            var existingPage = await _context.HomePages.FindAsync(homePage.Id);
            if (existingPage == null)
                return await CreateHomePageAsync(homePage);

            existingPage.SkillsSectionTitle = homePage.SkillsSectionTitle;
            existingPage.SkillsSectionSubtitle = homePage.SkillsSectionSubtitle;
            existingPage.SkillsCategories = homePage.SkillsCategories;
            existingPage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingPage;
        }

        public async Task<HomePage> AddSkillsCategoryAsync(int homePageId, SkillsCategory category)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new ArgumentException("HomePage not found");

            var categories = homePage.SkillsCategories;
            categories.Add(category);
            homePage.SkillsCategories = categories;
            homePage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return homePage;
        }

        public async Task<HomePage> UpdateSkillsCategoryAsync(int homePageId, SkillsCategory category)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new ArgumentException("HomePage not found");

            var categories = homePage.SkillsCategories;
            var index = categories.FindIndex(c => c.Title == category.Title);
            if (index != -1)
            {
                categories[index] = category;
                homePage.SkillsCategories = categories;
                homePage.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return homePage;
        }

        public async Task<HomePage> RemoveSkillsCategoryAsync(int homePageId, string categoryTitle)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new ArgumentException("HomePage not found");

            var categories = homePage.SkillsCategories;
            categories.RemoveAll(c => c.Title == categoryTitle);
            homePage.SkillsCategories = categories;
            homePage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return homePage;
        }

        public async Task<string> UploadSkillCategoryImageAsync(IFormFile file)
        {
            return await _fileService.UploadFileAsync(file, "homepage/skills");
        }

        // Featured Projects Section Operations
        public async Task<HomePage> UpdateFeaturedProjectsSectionAsync(HomePage homePage)
        {
            var existingPage = await _context.HomePages.FindAsync(homePage.Id);
            if (existingPage == null)
                return await CreateHomePageAsync(homePage);

            existingPage.FeaturedProjectsTitle = homePage.FeaturedProjectsTitle;
            existingPage.FeaturedProjectsSubtitle = homePage.FeaturedProjectsSubtitle;
            existingPage.FeaturedProjectIds = homePage.FeaturedProjectIds;
            existingPage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingPage;
        }

        public async Task<HomePage> AddFeaturedProjectAsync(int homePageId, int projectId)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new ArgumentException("HomePage not found");

            var projectIds = homePage.FeaturedProjectIds;
            if (!projectIds.Contains(projectId))
            {
                projectIds.Add(projectId);
                homePage.FeaturedProjectIds = projectIds;
                homePage.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return homePage;
        }

        public async Task<HomePage> RemoveFeaturedProjectAsync(int homePageId, int projectId)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new ArgumentException("HomePage not found");

            var projectIds = homePage.FeaturedProjectIds;
            projectIds.Remove(projectId);
            homePage.FeaturedProjectIds = projectIds;
            homePage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return homePage;
        }

        public async Task<HomePage> ReorderFeaturedProjectsAsync(int homePageId, List<int> projectIds)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new ArgumentException("HomePage not found");

            homePage.FeaturedProjectIds = projectIds;
            homePage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return homePage;
        }

        // CTA Section Operations
        public async Task<HomePage> UpdateCTASectionAsync(HomePage homePage)
        {
            var existingPage = await _context.HomePages.FindAsync(homePage.Id);
            if (existingPage == null)
                return await CreateHomePageAsync(homePage);

            existingPage.CTATitle = homePage.CTATitle;
            existingPage.CTASubtitle = homePage.CTASubtitle;
            existingPage.CTAButtonText = homePage.CTAButtonText;
            existingPage.CTAButtonUrl = homePage.CTAButtonUrl;
            existingPage.CTABackgroundImageUrl = homePage.CTABackgroundImageUrl;
            existingPage.CTATextColor = homePage.CTATextColor;
            existingPage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingPage;
        }

        public async Task<string> UploadCTABackgroundImageAsync(IFormFile file)
        {
            return await _fileService.UploadFileAsync(file, "homepage/cta");
        }

        // General Settings
        public async Task<HomePage> UpdateGeneralSettingsAsync(HomePage homePage)
        {
            var existingPage = await _context.HomePages.FindAsync(homePage.Id);
            if (existingPage == null)
                return await CreateHomePageAsync(homePage);

            existingPage.DisplayOrder = homePage.DisplayOrder;
            existingPage.IsActive = homePage.IsActive;
            existingPage.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingPage;
        }

        public async Task<bool> ToggleHomePageStatusAsync(int id)
        {
            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
                return false;

            homePage.IsActive = !homePage.IsActive;
            homePage.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<HomePage> UpdateDisplayOrderAsync(int id, int newOrder)
        {
            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
                throw new ArgumentException("HomePage not found");

            homePage.DisplayOrder = newOrder;
            homePage.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return homePage;
        }
    }
}