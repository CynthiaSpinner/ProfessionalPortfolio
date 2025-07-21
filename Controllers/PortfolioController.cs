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
            catch (Exception)
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
                
                // Return empty array instead of error for now
                return Json(new object[] { });
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
                
                // Return empty array instead of error for now
                return Json(new object[] { });
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
                // Return null instead of error for now
                return Json(null);
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
                        title = "Key Skills & Technologies",
                        subtitle = "Explore my expertise across different domains",
                        features = new[]
                        {
                            new
                            {
                                title = "Frontend Development",
                                subtitle = "React, JavaScript, HTML5, CSS3, Bootstrap",
                                description = "Building responsive and interactive user interfaces with modern frameworks and best practices.",
                                icon = "fas fa-code",
                                link = "/projects?category=frontend"
                            },
                            new
                            {
                                title = "Backend Development",
                                subtitle = ".NET Core, C#, RESTful APIs, SQL Server",
                                description = "Creating robust server-side applications and APIs with enterprise-grade technologies.",
                                icon = "fas fa-server",
                                link = "/projects?category=backend"
                            },
                            new
                            {
                                title = "Design & Tools",
                                subtitle = "Adobe Creative Suite, UI/UX Design, Git, Docker",
                                description = "Crafting beautiful designs and managing development workflows with professional tools.",
                                icon = "fas fa-palette",
                                link = "/projects?category=design"
                            }
                        },
                        lastModified = DateTime.UtcNow
                    });
                }

                return Json(new
                {
                    title = features.SectionTitle,
                    subtitle = features.SectionSubtitle,
                    features = new[]
                    {
                        new
                        {
                            title = features.Feature1Title,
                            subtitle = features.Feature1Subtitle,
                            description = features.Feature1Description,
                            icon = features.Feature1Icon,
                            link = features.Feature1Link
                        },
                        new
                        {
                            title = features.Feature2Title,
                            subtitle = features.Feature2Subtitle,
                            description = features.Feature2Description,
                            icon = features.Feature2Icon,
                            link = features.Feature2Link
                        },
                        new
                        {
                            title = features.Feature3Title,
                            subtitle = features.Feature3Subtitle,
                            description = features.Feature3Description,
                            icon = features.Feature3Icon,
                            link = features.Feature3Link
                        }
                    },
                    lastModified = features.UpdatedAt
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Failed to load features data" });
            }
        }


        // GET: api/portfolio/homepage - Consolidated endpoint for all homepage data
        [HttpGet("api/portfolio/homepage")]
        public async Task<IActionResult> GetHomepageData()
        {
            try
            {
                Console.WriteLine("Starting homepage data fetch...");
                
                var homePage = await _homePageService.GetHomePageAsync();
                Console.WriteLine($"HomePage fetched: {(homePage != null ? "Success" : "Null")}");
                
                var features = await _context.FeaturesSections.FirstOrDefaultAsync();
                Console.WriteLine($"Features fetched: {(features != null ? "Success" : "Null")}");

                // Build hero data - with fallback
                var heroData = homePage == null ? new
                {
                    title = "Welcome to My Portfolio",
                    subtitle = "I am a passionate software engineer specializing in full-stack development, with expertise in creating modern, scalable applications.",
                    description = "",
                    backgroundImageUrl = "",
                    backgroundVideoUrl = "",
                    primaryButtonText = "View Projects",
                    primaryButtonUrl = "/projects",
                    overlayColor = "#000000",
                    overlayOpacity = 0.5f,
                    lastModified = DateTime.UtcNow
                } : new
                {
                    title = homePage.HeaderTitle ?? "",
                    subtitle = homePage.HeaderSubtitle ?? "",
                    description = homePage.HeaderDescription ?? "",
                    backgroundImageUrl = homePage.HeaderBackgroundImageUrl ?? "",
                    backgroundVideoUrl = homePage.HeaderBackgroundVideoUrl ?? "",
                    primaryButtonText = homePage.HeaderPrimaryButtonText ?? "",
                    primaryButtonUrl = homePage.HeaderPrimaryButtonUrl ?? "",
                    overlayColor = homePage.HeaderOverlayColor ?? "",
                    overlayOpacity = homePage.HeaderOverlayOpacity ?? 0.5f,
                    lastModified = homePage.UpdatedAt ?? homePage.CreatedAt
                };

                // Build features data - with fallback
                var featuresData = features == null ? new
                {
                    title = "Key Skills & Technologies",
                    subtitle = "Explore my expertise across different domains",
                    features = new[]
                    {
                        new
                        {
                            title = "Frontend Development",
                            subtitle = "React, JavaScript, HTML5, CSS3, Bootstrap",
                            description = "Building responsive and interactive user interfaces with modern frameworks and best practices.",
                            icon = "fas fa-code",
                            link = "/projects?category=frontend"
                        },
                        new
                        {
                            title = "Backend Development",
                            subtitle = ".NET Core, C#, RESTful APIs, SQL Server",
                            description = "Creating robust server-side applications and APIs with enterprise-grade technologies.",
                            icon = "fas fa-server",
                            link = "/projects?category=backend"
                        },
                        new
                        {
                            title = "Design & Tools",
                            subtitle = "Adobe Creative Suite, UI/UX Design, Git, Docker",
                            description = "Crafting beautiful designs and managing development workflows with professional tools.",
                            icon = "fas fa-palette",
                            link = "/projects?category=design"
                        }
                    },
                    lastModified = DateTime.UtcNow
                } : new
                {
                    title = features.SectionTitle ?? "",
                    subtitle = features.SectionSubtitle ?? "",
                    features = new[]
                    {
                        new
                        {
                            title = features.Feature1Title ?? "",
                            subtitle = features.Feature1Subtitle ?? "",
                            description = features.Feature1Description ?? "",
                            icon = features.Feature1Icon ?? "",
                            link = features.Feature1Link ?? ""
                        },
                        new
                        {
                            title = features.Feature2Title ?? "",
                            subtitle = features.Feature2Subtitle ?? "",
                            description = features.Feature2Description ?? "",
                            icon = features.Feature2Icon ?? "",
                            link = features.Feature2Link ?? ""
                        },
                        new
                        {
                            title = features.Feature3Title ?? "",
                            subtitle = features.Feature3Subtitle ?? "",
                            description = features.Feature3Description ?? "",
                            icon = features.Feature3Icon ?? "",
                            link = features.Feature3Link ?? ""
                        }
                    },
                    lastModified = features.UpdatedAt ?? DateTime.UtcNow
                };

                return Json(new
                {
                    hero = heroData,
                    features = featuresData,
                    projects = new object[0], // Empty array instead of null
                    skills = new object[0],   // Empty array instead of null
                    about = (object)null,     // Explicit null cast
                    lastModified = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading homepage data: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                return StatusCode(500, new { error = "Failed to load homepage data", details = ex.Message });
            }
        }
    }
}