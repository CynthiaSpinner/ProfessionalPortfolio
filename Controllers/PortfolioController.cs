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
                var projects = await _context.Projects.ToListAsync();
                return Json(projects);
            }
            catch (Exception ex)
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
                var skillsCategories = await _context.SkillsCategories.ToListAsync();
                return Json(skillsCategories);
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
                var section = await _context.FeaturesSections.FirstOrDefaultAsync();
                if (section == null)
                {
                    return Json(new
                    {
                        sectionTitle = "Key Skills & Technologies",
                        sectionSubtitle = "Explore my expertise across different domains",
                        features = new[]
                        {
                            new { title = "Frontend Development", subtitle = "React, JavaScript, HTML5, CSS3, Bootstrap", description = "", icon = "fas fa-code", link = "/projects?category=frontend" },
                            new { title = "Backend Development", subtitle = ".NET Core, C#, RESTful APIs, SQL Server", description = "", icon = "fas fa-server", link = "/projects?category=backend" },
                            new { title = "Design & Tools", subtitle = "Adobe Creative Suite, UI/UX Design, Git, Docker", description = "", icon = "fas fa-palette", link = "/projects?category=design" }
                        },
                        lastModified = DateTime.UtcNow
                    });
                }
                return Json(new
                {
                    sectionTitle = section.SectionTitle,
                    sectionSubtitle = section.SectionSubtitle,
                    features = new[]
                    {
                        new { title = section.Feature1Title, subtitle = section.Feature1Subtitle, description = section.Feature1Description ?? "", icon = section.Feature1Icon ?? "", link = section.Feature1Link ?? "" },
                        new { title = section.Feature2Title, subtitle = section.Feature2Subtitle, description = section.Feature2Description ?? "", icon = section.Feature2Icon ?? "", link = section.Feature2Link ?? "" },
                        new { title = section.Feature3Title, subtitle = section.Feature3Subtitle, description = section.Feature3Description ?? "", icon = section.Feature3Icon ?? "", link = section.Feature3Link ?? "" }
                    },
                    lastModified = section.UpdatedAt
                });
            }
            catch (Exception ex)
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
                var section = await _context.CTASections.FirstOrDefaultAsync();
                if (section == null)
                {
                    return Json(new
                    {
                        title = "Ready to Start a Project?",
                        subtitle = "Let's work together to bring your ideas to life.",
                        buttonText = "Get in Touch",
                        buttonLink = "/contact",
                        lastModified = DateTime.UtcNow
                    });
                }
                return Json(new
                {
                    title = section.Title,
                    subtitle = section.Subtitle,
                    buttonText = section.ButtonText,
                    buttonLink = section.ButtonLink,
                    lastModified = section.UpdatedAt ?? section.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to load CTA data" });
            }
        }
    }
}