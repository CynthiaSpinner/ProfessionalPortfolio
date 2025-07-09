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

        // GET: Portfolio/About
        public async Task<IActionResult> About()
        {
            var about = await _context.Abouts.FirstOrDefaultAsync();
            return View(about);
        }
    }
}