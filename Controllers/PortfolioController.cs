using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Data.Repositories;
using Portfolio.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly IHomePageService _homePageService;
        private readonly ISkillsCategoryRepository _skillsCategoryRepository;
        private readonly ISiteSettingsRepository _siteSettingsRepository;
        private readonly IFeaturesSectionRepository _featuresSectionRepository;
        private readonly IFeaturesSectionService _featuresSectionService;
        private readonly IPortfolioPublicService _portfolioPublicService;
        private readonly ICTASectionRepository _ctaSectionRepository;
        private readonly ICTASectionService _ctaSectionService;
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(
            PortfolioContext context,
            IHomePageService homePageService,
            ISkillsCategoryRepository skillsCategoryRepository,
            ISiteSettingsRepository siteSettingsRepository,
            IFeaturesSectionRepository featuresSectionRepository,
            IFeaturesSectionService featuresSectionService,
            IPortfolioPublicService portfolioPublicService,
            ICTASectionRepository ctaSectionRepository,
            ICTASectionService ctaSectionService,
            ILogger<PortfolioController> logger)
        {
            _context = context;
            _homePageService = homePageService;
            _skillsCategoryRepository = skillsCategoryRepository;
            _siteSettingsRepository = siteSettingsRepository;
            _featuresSectionRepository = featuresSectionRepository;
            _featuresSectionService = featuresSectionService;
            _portfolioPublicService = portfolioPublicService;
            _ctaSectionRepository = ctaSectionRepository;
            _ctaSectionService = ctaSectionService;
            _logger = logger;
        }

        // GET: Portfolio/Projects
        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        // GET: Portfolio/Skills
        public async Task<IActionResult> Skills()
        {
            var skillsCategories = await _skillsCategoryRepository.GetAllOrderedAsync();
            return View(skillsCategories);
        }

        // GET: Portfolio/Home
        public async Task<IActionResult> Home()
        {
            var homePage = await _homePageService.GetHomePageAsync();
            return View(homePage);
        }

        private static object GetDefaultHeroJson(DateTime lastModified)
        {
            return new
            {
                title = "Welcome to My Portfolio",
                subtitle = "I am a passionate software engineer specializing in full-stack development, with expertise in creating modern, scalable applications.",
                description = "",
                backgroundImageUrl = "",
                backgroundVideoUrl = "",
                primaryButtonText = "View Projects",
                primaryButtonUrl = "/projects",
                overlayColor = "#000000",
                overlayOpacity = 0.5,
                lastModified
            };
        }

        // GET: api/portfolio/hero
        [HttpGet("api/portfolio/hero")]
        public async Task<IActionResult> GetHeroSection()
        {
            try
            {
                var homePage = await _homePageService.GetHomePageAsync();
                
                if (homePage == null)
                    return Json(GetDefaultHeroJson(DateTime.UtcNow));

                var scheme = Request.Scheme;
                if (scheme == "http" && Request.Host.Host?.Contains("onrender.com", StringComparison.OrdinalIgnoreCase) == true)
                    scheme = "https";
                var baseUrl = $"{scheme}://{Request.Host}";
                var version = (homePage.UpdatedAt ?? homePage.CreatedAt).Ticks;
                var backgroundImageUrl = homePage.HeaderBackgroundImageData != null
                    ? $"{baseUrl}/api/portfolio/hero-image?v={version}"
                    : (homePage.HeaderBackgroundImageUrl ?? "");

                return Json(new
                {
                    title = homePage.HeaderTitle,
                    subtitle = homePage.HeaderSubtitle,
                    description = homePage.HeaderDescription,
                    backgroundImageUrl,
                    backgroundVideoUrl = homePage.HeaderBackgroundVideoUrl,
                    primaryButtonText = homePage.HeaderPrimaryButtonText,
                    primaryButtonUrl = homePage.HeaderPrimaryButtonUrl,
                    overlayColor = homePage.HeaderOverlayColor,
                    overlayOpacity = homePage.HeaderOverlayOpacity,
                    lastModified = homePage.UpdatedAt ?? homePage.CreatedAt
                });
            }
            catch (Exception ex)
            {
                // Log so Render/server logs show the real cause (e.g. missing migration columns)
                // Return 200 with default hero so the site still loads; run DB migration to fix.
                _logger.LogError(ex, "GetHeroSection failed: {Message}", ex.Message);
                return Json(GetDefaultHeroJson(DateTime.UtcNow));
            }
        }

        // GET: api/portfolio/hero-image (serves stored hero background image from DB)
        [HttpGet("api/portfolio/hero-image")]
        public async Task<IActionResult> GetHeroImage()
        {
            try
            {
                var homePage = await _homePageService.GetHomePageAsync();
                if (homePage?.HeaderBackgroundImageData == null || homePage.HeaderBackgroundImageData.Length == 0)
                    return NotFound();
                var contentType = homePage.HeaderBackgroundImageContentType ?? "image/jpeg";
                Response.Headers.CacheControl = "public, max-age=31536000, immutable";
                return File(homePage.HeaderBackgroundImageData, contentType);
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: api/portfolio/projects
        [HttpGet("api/portfolio/projects")]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var projects = await _context.Projects.ToListAsync();
                return Json(projects);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Failed to load projects data" });
            }
        }

        // GET: api/portfolio/skills
        [HttpGet("api/portfolio/skills")]
        public async Task<IActionResult> GetSkills()
        {
            try
            {
                var dto = await _portfolioPublicService.GetSkillsForPublicAsync();
                return Json(new { categories = dto.Categories, linkText = dto.LinkText });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Failed to load skills data" });
            }
        }

        // GET: api/portfolio/about
        [HttpGet("api/portfolio/about")]
        public async Task<IActionResult> GetAbout()
        {
            try
            {
                var about = await _context.Abouts.FirstOrDefaultAsync();
                return Json(about);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Failed to load about data" });
            }
        }

        // GET: api/portfolio/features (public - for frontend home page)
        [HttpGet("api/portfolio/features")]
        public async Task<IActionResult> GetFeatures()
        {
            try
            {
                var result = await _featuresSectionService.GetForPublicApiAsync();
                return Json(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Failed to load features data" });
            }
        }

        // GET: api/portfolio/cta (public - for frontend home page)
        [HttpGet("api/portfolio/cta")]
        public async Task<IActionResult> GetCTA()
        {
            try
            {
                var result = await _ctaSectionService.GetForPublicApiAsync();
                return Json(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Failed to load CTA data" });
            }
        }

        // GET: api/portfolio/nav-settings (public - which nav links to show)
        [HttpGet("api/portfolio/nav-settings")]
        public async Task<IActionResult> GetNavSettings()
        {
            try
            {
                var settings = await _siteSettingsRepository.GetFirstOrDefaultAsync();
                if (settings == null)
                    return Json(new { showGraphicDesignLink = true, showDesignLink = true });
                return Json(new
                {
                    showGraphicDesignLink = settings.ShowGraphicDesignLink,
                    showDesignLink = settings.ShowDesignLink
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Nav settings not available");
                return Json(new { showGraphicDesignLink = true, showDesignLink = true });
            }
        }
    }
}