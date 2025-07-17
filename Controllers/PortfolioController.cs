using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly IHomePageService _homePageService;

        public PortfolioController(PortfolioContext context, IHomePageService homePageService)
        {
            _context = context;
            _homePageService = homePageService;
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
            var skillsCategories = await _context.SkillsCategories.ToListAsync();
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
                var homePage = await _homePageService.GetHomePageAsync();
                
                if (homePage == null)
                {
                    return Json(new
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
                        lastModified = DateTime.UtcNow
                    });
                }

                return Json(new
                {
                    title = homePage.HeaderTitle,
                    subtitle = homePage.HeaderSubtitle,
                    description = homePage.HeaderDescription,
                    backgroundImageUrl = homePage.HeaderBackgroundImageUrl,
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
                return StatusCode(500, new { error = "Failed to load hero section data" });
            }
        }

        // GET: api/portfolio/projects
        [HttpGet("api/portfolio/projects")]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                // Check if table exists and has data
                var projectCount = await _context.Projects.CountAsync();
                Console.WriteLine($"Found {projectCount} projects in database");
                
                var projects = await _context.Projects.ToListAsync();
                return Json(projects);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading projects: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = $"Failed to load projects data: {ex.Message}" });
            }
        }

        // GET: api/portfolio/skills
        [HttpGet("api/portfolio/skills")]
        public async Task<IActionResult> GetSkills()
        {
            try
            {
                // Check if table exists and has data
                var skillsCount = await _context.SkillsCategories.CountAsync();
                Console.WriteLine($"Found {skillsCount} skills categories in database");
                
                var skillsCategories = await _context.SkillsCategories.ToListAsync();
                return Json(skillsCategories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading skills: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = $"Failed to load skills data: {ex.Message}" });
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
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to load about data" });
            }
        }

        // GET: api/portfolio/features
        [HttpGet("api/portfolio/features")]
        public async Task<IActionResult> GetFeatures()
        {
            try
            {
                var features = await _context.FeaturesSections.FirstOrDefaultAsync();
                
                if (features == null)
                {
                    // Return default features if none exist
                    return Json(new
                    {
                        sectionTitle = "Key Skills & Technologies",
                        feature1Title = "Frontend Development",
                        feature1Subtitle = "React, JavaScript, HTML5, CSS3, Bootstrap",
                        feature2Title = "Backend Development",
                        feature2Subtitle = ".NET Core, C#, RESTful APIs, MySQL",
                        feature3Title = "Design & Tools",
                        feature3Subtitle = "Adobe Creative Suite, UI/UX Design, Git, Docker",
                        lastModified = DateTime.UtcNow
                    });
                }

                return Json(new
                {
                    sectionTitle = features.SectionTitle,
                    feature1Title = features.Feature1Title,
                    feature1Subtitle = features.Feature1Subtitle,
                    feature2Title = features.Feature2Title,
                    feature2Subtitle = features.Feature2Subtitle,
                    feature3Title = features.Feature3Title,
                    feature3Subtitle = features.Feature3Subtitle,
                    lastModified = features.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to load features data" });
            }
        }
    }
}