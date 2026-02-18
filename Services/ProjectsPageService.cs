using Portfolio.Data.Repositories;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services
{
    public class ProjectsPageService : IProjectsPageService
    {
        private readonly IProjectsPageHeroRepository _heroRepo;
        private readonly IProjectsPageCTARepository _ctaRepo;

        public ProjectsPageService(
            IProjectsPageHeroRepository heroRepo,
            IProjectsPageCTARepository ctaRepo)
        {
            _heroRepo = heroRepo;
            _ctaRepo = ctaRepo;
        }

        public async Task<object?> GetHeroForPublicApiAsync()
        {
            var h = await _heroRepo.GetFirstOrDefaultAsync();
            if (h == null) return null;
            return new
            {
                title = h.Title ?? "My Projects",
                subtitle = h.Subtitle ?? "",
                buttonText = h.ButtonText ?? "About me",
                buttonUrl = h.ButtonUrl ?? "/about",
                lastModified = h.UpdatedAt ?? h.CreatedAt
            };
        }

        public async Task<object?> GetHeroForAdminAsync()
        {
            var h = await _heroRepo.GetFirstOrDefaultAsync();
            if (h == null) return null;
            return new
            {
                id = h.Id,
                title = h.Title,
                subtitle = h.Subtitle ?? "",
                buttonText = h.ButtonText ?? "",
                buttonUrl = h.ButtonUrl ?? "",
                lastModified = h.UpdatedAt ?? h.CreatedAt
            };
        }

        public async Task<(bool Success, string Message)> SaveHeroAsync(string title, string? subtitle, string? buttonText, string? buttonUrl)
        {
            var h = await _heroRepo.GetFirstOrDefaultAsync();
            if (h == null)
            {
                h = new ProjectsPageHero
                {
                    Title = title?.Trim() ?? "My Projects",
                    Subtitle = subtitle?.Trim(),
                    ButtonText = buttonText?.Trim(),
                    ButtonUrl = buttonUrl?.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _heroRepo.AddAsync(h);
            }
            else
            {
                h.Title = title?.Trim() ?? "My Projects";
                h.Subtitle = string.IsNullOrWhiteSpace(subtitle) ? null : subtitle.Trim();
                h.ButtonText = string.IsNullOrWhiteSpace(buttonText) ? null : buttonText.Trim();
                h.ButtonUrl = string.IsNullOrWhiteSpace(buttonUrl) ? null : buttonUrl.Trim();
                await _heroRepo.UpdateAsync(h);
            }
            return (true, "Projects page hero saved.");
        }

        public async Task<object> GetCTAForPublicApiAsync()
        {
            var c = await _ctaRepo.GetFirstOrDefaultAsync();
            if (c == null)
            {
                return new
                {
                    title = "Ready to work together?",
                    subtitle = "Let's build something.",
                    buttonText = "Get in Touch",
                    buttonLink = "/contact",
                    lastModified = DateTime.UtcNow
                };
            }
            return new
            {
                title = c.Title,
                subtitle = c.Subtitle ?? "",
                buttonText = c.ButtonText ?? "Get in Touch",
                buttonLink = c.ButtonLink ?? "/contact",
                lastModified = c.UpdatedAt ?? c.CreatedAt
            };
        }

        public async Task<object?> GetCTAForAdminAsync()
        {
            var c = await _ctaRepo.GetFirstOrDefaultAsync();
            if (c == null) return null;
            return new
            {
                id = c.Id,
                title = c.Title,
                subtitle = c.Subtitle ?? "",
                buttonText = c.ButtonText ?? "",
                buttonLink = c.ButtonLink ?? "",
                lastModified = c.UpdatedAt ?? c.CreatedAt
            };
        }

        public async Task<(bool Success, string Message)> SaveCTAAsync(string title, string subtitle, string buttonText, string buttonLink)
        {
            var c = await _ctaRepo.GetFirstOrDefaultAsync();
            if (c == null)
            {
                c = new ProjectsPageCTA
                {
                    Title = title?.Trim() ?? "Ready to work together?",
                    Subtitle = subtitle?.Trim() ?? "",
                    ButtonText = buttonText?.Trim() ?? "Get in Touch",
                    ButtonLink = buttonLink?.Trim() ?? "/contact",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _ctaRepo.AddAsync(c);
            }
            else
            {
                c.Title = title?.Trim() ?? "Ready to work together?";
                c.Subtitle = subtitle?.Trim() ?? "";
                c.ButtonText = buttonText?.Trim() ?? "Get in Touch";
                c.ButtonLink = buttonLink?.Trim() ?? "/contact";
                await _ctaRepo.UpdateAsync(c);
            }
            return (true, "Projects page CTA saved.");
        }
    }
}
