using Microsoft.AspNetCore.Mvc;
using Portfolio.Services.Interfaces;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly IHomePageService _homePageService;
        private readonly IFeaturesSectionService _featuresSectionService;
        private readonly IPortfolioPublicService _portfolioPublicService;
        private readonly ICTASectionService _ctaSectionService;
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(
            IHomePageService homePageService,
            IFeaturesSectionService featuresSectionService,
            IPortfolioPublicService portfolioPublicService,
            ICTASectionService ctaSectionService,
            ILogger<PortfolioController> logger)
        {
            _homePageService = homePageService;
            _featuresSectionService = featuresSectionService;
            _portfolioPublicService = portfolioPublicService;
            _ctaSectionService = ctaSectionService;
            _logger = logger;
        }

        // GET: Portfolio/Projects
        public async Task<IActionResult> Projects()
        {
            var projects = await _portfolioPublicService.GetProjectsAsync();
            return View(projects);
        }

        // GET: Portfolio/Skills
        public async Task<IActionResult> Skills()
        {
            var skillsCategories = await _portfolioPublicService.GetSkillsCategoriesOrderedAsync();
            return View(skillsCategories);
        }

        // GET: Portfolio/Home
        public async Task<IActionResult> Home()
        {
            var homePage = await _homePageService.GetHomePageAsync();
            return View(homePage);
        }

        // GET: api/portfolio/hero
        [HttpGet("api/portfolio/hero")]
        public async Task<IActionResult> GetHeroSection()
        {
            try
            {
                var scheme = Request.Scheme;
                if (scheme == "http" && Request.Host.Host?.Contains("onrender.com", StringComparison.OrdinalIgnoreCase) == true)
                    scheme = "https";
                var baseUrl = $"{scheme}://{Request.Host}";
                var result = await _homePageService.GetHeroForPublicApiAsync(baseUrl);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetHeroSection failed: {Message}", ex.Message);
                var fallback = await _homePageService.GetHeroForPublicApiAsync("");
                return Json(fallback);
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
                var projects = await _portfolioPublicService.GetProjectsAsync();
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
                var about = await _portfolioPublicService.GetAboutAsync();
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
                var dto = await _portfolioPublicService.GetNavSettingsAsync();
                return Json(new { showGraphicDesignLink = dto.ShowGraphicDesignLink, showDesignLink = dto.ShowDesignLink });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Nav settings not available");
                return Json(new { showGraphicDesignLink = true, showDesignLink = true });
            }
        }
    }
}