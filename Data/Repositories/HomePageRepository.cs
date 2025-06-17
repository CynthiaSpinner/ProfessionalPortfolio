using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;

namespace Portfolio.Data.Repositories
{
    public class HomePageRepository : IHomePageService
    {
        private readonly PortfolioContext _context;

        public HomePageRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task<HomePage?> GetHomePageAsync()
        {
            return await _context.HomePages
                .FirstOrDefaultAsync(hp => hp.IsActive);
        }

        public async Task<HomePage?> GetHomePageByIdAsync(int id)
        {
            return await _context.HomePages
                .FirstOrDefaultAsync(hp => hp.Id == id);
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

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<HomePage> UpdateHeaderSectionAsync(HomePage homePage)
        {
            return await UpdateHomePageAsync(homePage);
        }

        public async Task<string> UploadHeaderBackgroundImageAsync(IFormFile file)
        {
            // Implementation will be added later
            throw new NotImplementedException();
        }

        public async Task<string> UploadHeaderBackgroundVideoAsync(IFormFile file)
        {
            // Implementation will be added later
            throw new NotImplementedException();
        }

        public async Task<HomePage> UpdateSkillsSectionAsync(HomePage homePage)
        {
            return await UpdateHomePageAsync(homePage);
        }

        public async Task<HomePage> AddSkillsCategoryAsync(int homePageId, SkillsCategory category)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new Exception("HomePage not found");

            homePage.SkillsCategories.Add(category);
            await _context.SaveChangesAsync();
            return homePage;
        }

        public async Task<HomePage> UpdateSkillsCategoryAsync(int homePageId, SkillsCategory category)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new Exception("HomePage not found");

            var existingCategory = homePage.SkillsCategories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory == null)
                throw new Exception("Skills category not found");

            existingCategory.Title = category.Title;
            existingCategory.Description = category.Description;
            existingCategory.ImagePath = category.ImagePath;
            existingCategory.Skills = category.Skills;

            await _context.SaveChangesAsync();
            return homePage;
        }

        public async Task<HomePage> RemoveSkillsCategoryAsync(int homePageId, string categoryTitle)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new Exception("HomePage not found");

            var category = homePage.SkillsCategories.FirstOrDefault(c => c.Title == categoryTitle);
            if (category != null)
            {
                homePage.SkillsCategories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return homePage;
        }

        public async Task<string> UploadSkillCategoryImageAsync(IFormFile file)
        {
            // Implementation will be added later
            throw new NotImplementedException();
        }

        public async Task<HomePage> UpdateFeaturedProjectsSectionAsync(HomePage homePage)
        {
            return await UpdateHomePageAsync(homePage);
        }

        public async Task<HomePage> AddFeaturedProjectAsync(int homePageId, int projectId)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new Exception("HomePage not found");

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                throw new Exception("Project not found");

            homePage.FeaturedProjects.Add(project);
            await _context.SaveChangesAsync();
            return homePage;
        }

        public async Task<HomePage> RemoveFeaturedProjectAsync(int homePageId, int projectId)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new Exception("HomePage not found");

            var project = homePage.FeaturedProjects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                homePage.FeaturedProjects.Remove(project);
                await _context.SaveChangesAsync();
            }

            return homePage;
        }

        public async Task<HomePage> ReorderFeaturedProjectsAsync(int homePageId, List<int> projectIds)
        {
            var homePage = await _context.HomePages.FindAsync(homePageId);
            if (homePage == null)
                throw new Exception("HomePage not found");

            // Implementation will be added later
            throw new NotImplementedException();
        }

        public async Task<HomePage> UpdateCTASectionAsync(HomePage homePage)
        {
            return await UpdateHomePageAsync(homePage);
        }

        public async Task<string> UploadCTABackgroundImageAsync(IFormFile file)
        {
            // Implementation will be added later
            throw new NotImplementedException();
        }

        public async Task<HomePage> UpdateGeneralSettingsAsync(HomePage homePage)
        {
            return await UpdateHomePageAsync(homePage);
        }

        public async Task<bool> ToggleHomePageStatusAsync(int id)
        {
            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
                return false;

            homePage.IsActive = !homePage.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<HomePage> UpdateDisplayOrderAsync(int id, int newOrder)
        {
            var homePage = await _context.HomePages.FindAsync(id);
            if (homePage == null)
                throw new Exception("HomePage not found");

            homePage.DisplayOrder = newOrder;
            await _context.SaveChangesAsync();
            return homePage;
        }
    }
}