using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services;
using Portfolio.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace Portfolio.Controllers
{
    public class AdminController : Controller
    {
        private readonly PortfolioContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHomePageService _homePageService;
        private readonly WebSocketService _webSocketService;

        public AdminController(PortfolioContext context, ILogger<AdminController> logger, IConfiguration configuration, IHomePageService homePageService, WebSocketService webSocketService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _homePageService = homePageService;
            _webSocketService = webSocketService;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            try
            {
                _logger.LogInformation("Login page requested");
                
                if (User.Identity.IsAuthenticated)
                {
                    _logger.LogInformation("User already authenticated, redirecting to dashboard");
                    return RedirectToAction("Dashboard");
                }
                
                // Clear any existing error messages
                ViewBag.ErrorMessage = null;
                _logger.LogInformation("Login page loaded successfully");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading login page");
                ViewBag.ErrorMessage = $"Error loading login page: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Please provide valid username and password.";
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    ViewBag.ErrorMessage = "Username and password are required.";
                    return View(model);
                }

                var hashedPassword = HashPassword(model.Password);

                // Optimized query - only select the fields we need
                var admin = await _context.Admins
                    .Where(a => a.Username == model.Username)
                    .Select(a => new { a.Username, a.PasswordHash })
                    .FirstOrDefaultAsync();

                if (admin == null || admin.PasswordHash != hashedPassword)
                {
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ViewBag.ErrorMessage = $"An error occurred during login: {ex.Message}";
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var homePage = await _homePageService.GetHomePageAsync();
                ViewBag.HomePage = homePage;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard");
                ViewBag.HomePage = null;
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                // Test database connection
                var homePageCount = await _context.HomePages.CountAsync();
                var adminCount = await _context.Admins.CountAsync();
                var templateCount = await _context.HeroTemplates.CountAsync();
                
                // Get template details if any exist
                var templates = await _context.HeroTemplates
                    .Select(t => new { t.Id, t.Nickname, t.HeaderTitle })
                    .ToListAsync();
                
                return Json(new { 
                    success = true, 
                    message = "Database connection test successful",
                    data = new {
                        homePages = homePageCount,
                        admins = adminCount,
                        heroTemplates = templateCount,
                        templates = templates
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection test failed: {Message}", ex.Message);
                return Json(new { 
                    success = false, 
                    message = $"Database connection test failed: {ex.Message}"
                });
            }
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHeroSection([FromForm] HeroSectionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Single database query - get or create home page
                var homePage = await _homePageService.GetHomePageAsync();
                
                if (homePage == null)
                {
                    homePage = new HomePage();
                    _context.HomePages.Add(homePage);
                }

                // Update hero section properties
                homePage.HeaderTitle = model.HeaderTitle ?? "Welcome to My Portfolio";
                homePage.HeaderSubtitle = model.HeaderSubtitle ?? "I am a passionate software engineer specializing in full-stack development.";
                homePage.HeaderDescription = model.HeaderDescription ?? "";
                homePage.HeaderBackgroundImageUrl = model.HeaderBackgroundImageUrl ?? "";
                homePage.HeaderBackgroundVideoUrl = model.HeaderBackgroundVideoUrl ?? "";
                homePage.HeaderPrimaryButtonText = model.HeaderPrimaryButtonText ?? "View Projects";
                homePage.HeaderPrimaryButtonUrl = model.HeaderPrimaryButtonUrl ?? "/projects";
                homePage.HeaderOverlayColor = model.ImageOverlayColor ?? "#000000";
                homePage.HeaderOverlayOpacity = model.ImageOverlayOpacity / 100f;
                homePage.IsActive = true; // Ensure the home page is active
                homePage.UpdatedAt = DateTime.UtcNow;

                // Single save operation
                await _context.SaveChangesAsync();

                // Force cache refresh to ensure fresh data (only if it's the service with caching)
                if (_homePageService is Services.HomePageService homePageService)
                {
                    homePageService.ForceCacheRefresh();
                }
                var updatedHomePage = await _homePageService.GetHomePageAsync();

                // Broadcast WebSocket message to all connected clients
                await _webSocketService.BroadcastHeroDataUpdatedAsync();

                // Return updated data for immediate frontend refresh
                return Json(new { 
                    success = true, 
                    message = "Hero section saved successfully!",
                    data = new
                    {
                        title = updatedHomePage?.HeaderTitle ?? homePage.HeaderTitle,
                        subtitle = updatedHomePage?.HeaderSubtitle ?? homePage.HeaderSubtitle,
                        description = updatedHomePage?.HeaderDescription ?? homePage.HeaderDescription,
                        backgroundImageUrl = updatedHomePage?.HeaderBackgroundImageUrl ?? homePage.HeaderBackgroundImageUrl,
                        backgroundVideoUrl = updatedHomePage?.HeaderBackgroundVideoUrl ?? homePage.HeaderBackgroundVideoUrl,
                        primaryButtonText = updatedHomePage?.HeaderPrimaryButtonText ?? homePage.HeaderPrimaryButtonText,
                        primaryButtonUrl = updatedHomePage?.HeaderPrimaryButtonUrl ?? homePage.HeaderPrimaryButtonUrl,
                        overlayColor = updatedHomePage?.HeaderOverlayColor ?? homePage.HeaderOverlayColor,
                        overlayOpacity = updatedHomePage?.HeaderOverlayOpacity ?? homePage.HeaderOverlayOpacity,
                        lastModified = updatedHomePage?.UpdatedAt ?? homePage.UpdatedAt ?? homePage.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving hero section");
                return Json(new { 
                    success = false, 
                    message = $"An error occurred while updating database: {ex.Message}",
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHeroTemplate([FromForm] HeroTemplateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Check if template with this nickname already exists
                var existingTemplate = await _context.HeroTemplates
                    .FirstOrDefaultAsync(t => t.Nickname == model.Nickname);
                
                if (existingTemplate != null)
                {
                    return Json(new { success = false, message = "A template with this nickname already exists. Please choose a different name." });
                }

                // Create new hero template
                var template = new HeroTemplate
                {
                    Nickname = model.Nickname,
                    HeaderTitle = model.HeaderTitle ?? "",
                    HeaderSubtitle = model.HeaderSubtitle ?? "",
                    HeaderDescription = model.HeaderDescription ?? "",
                    HeaderBackgroundImageUrl = model.HeaderBackgroundImageUrl ?? "",
                    HeaderBackgroundVideoUrl = model.HeaderBackgroundVideoUrl ?? "",
                    HeaderPrimaryButtonText = model.HeaderPrimaryButtonText ?? "",
                    HeaderPrimaryButtonUrl = model.HeaderPrimaryButtonUrl ?? "",
                    HeaderOverlayColor = model.ImageOverlayColor ?? "#000000",
                    HeaderOverlayOpacity = model.ImageOverlayOpacity / 100f,
                    CreatedAt = DateTime.UtcNow
                };

                _context.HeroTemplates.Add(template);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Hero template saved successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving hero template");
                return Json(new { success = false, message = "An error occurred while saving the hero template." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHeroTemplate([FromForm] HeroTemplateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Find existing template by ID
                var existingTemplate = await _context.HeroTemplates.FindAsync(model.Id);
                
                if (existingTemplate == null)
                {
                    return Json(new { success = false, message = "Template not found." });
                }

                // Update template properties
                existingTemplate.Nickname = model.Nickname;
                existingTemplate.HeaderTitle = model.HeaderTitle ?? "";
                existingTemplate.HeaderSubtitle = model.HeaderSubtitle ?? "";
                existingTemplate.HeaderDescription = model.HeaderDescription ?? "";
                existingTemplate.HeaderBackgroundImageUrl = model.HeaderBackgroundImageUrl ?? "";
                existingTemplate.HeaderBackgroundVideoUrl = model.HeaderBackgroundVideoUrl ?? "";
                existingTemplate.HeaderPrimaryButtonText = model.HeaderPrimaryButtonText ?? "";
                existingTemplate.HeaderPrimaryButtonUrl = model.HeaderPrimaryButtonUrl ?? "";
                existingTemplate.HeaderOverlayColor = model.ImageOverlayColor ?? "#000000";
                existingTemplate.HeaderOverlayOpacity = model.ImageOverlayOpacity / 100f;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Hero template updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating hero template");
                return Json(new { success = false, message = "An error occurred while updating the hero template." });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHeroTemplates()
        {
            try
            {
                // Check if the table exists by trying to access it
                var templateCount = await _context.HeroTemplates.CountAsync();
                _logger.LogInformation($"Found {templateCount} hero templates in database");
                
                var templates = await _context.HeroTemplates
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => new
                    {
                        id = t.Id,
                        nickname = t.Nickname,
                        title = t.HeaderTitle,
                        subtitle = t.HeaderSubtitle,
                        createdAt = t.CreatedAt
                    })
                    .ToListAsync();

                return Json(new { 
                    success = true, 
                    message = "Hero templates retrieved successfully.",
                    data = templates 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving hero templates: {Message}", ex.Message);
                return Json(new { 
                    success = false, 
                    message = $"An error occurred while retrieving hero templates: {ex.Message}",
                    data = new object[] { } 
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHeroTemplate(int id)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve hero template with ID: {id}");
                
                var template = await _context.HeroTemplates.FindAsync(id);
                
                if (template == null)
                {
                    _logger.LogWarning($"Hero template with ID {id} not found");
                    return Json(new { 
                        success = false, 
                        message = "Template not found.",
                        data = (object)null 
                    });
                }

                _logger.LogInformation($"Found template: {template.Nickname}");

                var templateData = new
                {
                    nickname = template.Nickname,
                    title = template.HeaderTitle,
                    subtitle = template.HeaderSubtitle,
                    description = template.HeaderDescription,
                    backgroundImageUrl = template.HeaderBackgroundImageUrl,
                    backgroundVideoUrl = template.HeaderBackgroundVideoUrl,
                    primaryButtonText = template.HeaderPrimaryButtonText,
                    primaryButtonUrl = template.HeaderPrimaryButtonUrl,
                    overlayColor = template.HeaderOverlayColor,
                    overlayOpacity = template.HeaderOverlayOpacity,
                    createdAt = template.CreatedAt
                };

                return Json(new { 
                    success = true, 
                    message = "Hero template retrieved successfully.",
                    data = templateData 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving hero template with ID {Id}: {Message}", id, ex.Message);
                return Json(new { 
                    success = false, 
                    message = $"An error occurred while retrieving the hero template: {ex.Message}",
                    data = (object)null 
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHeroTemplate(int id)
        {
            try
            {
                var template = await _context.HeroTemplates.FindAsync(id);
                
                if (template == null)
                {
                    return Json(new { success = false, message = "Template not found." });
                }

                _context.HeroTemplates.Remove(template);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Hero template deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting hero template");
                return Json(new { success = false, message = "An error occurred while deleting the hero template." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFeaturesSection([FromForm] FeaturesSectionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Get existing features section or create new one
                var featuresSection = await _context.FeaturesSections.FirstOrDefaultAsync();
                
                if (featuresSection == null)
                {
                    featuresSection = new FeaturesSection();
                    _context.FeaturesSections.Add(featuresSection);
                }

                // Update features section properties
                featuresSection.SectionTitle = model.SectionTitle ?? "Key Skills & Technologies";
                featuresSection.SectionSubtitle = model.SectionSubtitle ?? "Explore my expertise across different domains";
                featuresSection.Feature1Title = model.Feature1Title ?? "Frontend Development";
                featuresSection.Feature1Subtitle = model.Feature1Subtitle ?? "React, JavaScript, HTML5, CSS3, Bootstrap";
                featuresSection.Feature1Description = model.Feature1Description ?? "Building responsive and interactive user interfaces with modern frameworks and best practices.";
                featuresSection.Feature1Icon = model.Feature1Icon ?? "fas fa-code";
                featuresSection.Feature1Link = model.Feature1Link ?? "/projects?category=frontend";
                featuresSection.Feature2Title = model.Feature2Title ?? "Backend Development";
                featuresSection.Feature2Subtitle = model.Feature2Subtitle ?? ".NET Core, C#, RESTful APIs, SQL Server";
                featuresSection.Feature2Description = model.Feature2Description ?? "Creating robust server-side applications and APIs with enterprise-grade technologies.";
                featuresSection.Feature2Icon = model.Feature2Icon ?? "fas fa-server";
                featuresSection.Feature2Link = model.Feature2Link ?? "/projects?category=backend";
                featuresSection.Feature3Title = model.Feature3Title ?? "Design & Tools";
                featuresSection.Feature3Subtitle = model.Feature3Subtitle ?? "Adobe Creative Suite, UI/UX Design, Git, Docker";
                featuresSection.Feature3Description = model.Feature3Description ?? "Crafting beautiful designs and managing development workflows with professional tools.";
                featuresSection.Feature3Icon = model.Feature3Icon ?? "fas fa-palette";
                featuresSection.Feature3Link = model.Feature3Link ?? "/projects?category=design";
                featuresSection.IsActive = model.IsActive;
                featuresSection.DisplayOrder = model.DisplayOrder;
                featuresSection.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Broadcast WebSocket message for real-time updates
                await _webSocketService.BroadcastMessageAsync(System.Text.Json.JsonSerializer.Serialize(new { type = "featuresDataUpdated" }));

                return Json(new { success = true, message = "Features section saved successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving features section");
                return Json(new { success = false, message = "An error occurred while saving the features section." });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFeaturesSection()
        {
            try
            {
                var featuresSection = await _context.FeaturesSections.FirstOrDefaultAsync();
                
                if (featuresSection == null)
                {
                    return Json(new { 
                        success = false, 
                        message = "Features section not found.",
                        data = (object)null 
                    });
                }

                var featuresData = new
                {
                    sectionTitle = featuresSection.SectionTitle,
                    sectionSubtitle = featuresSection.SectionSubtitle,
                    feature1Title = featuresSection.Feature1Title,
                    feature1Subtitle = featuresSection.Feature1Subtitle,
                    feature1Description = featuresSection.Feature1Description,
                    feature1Icon = featuresSection.Feature1Icon,
                    feature1Link = featuresSection.Feature1Link,
                    feature2Title = featuresSection.Feature2Title,
                    feature2Subtitle = featuresSection.Feature2Subtitle,
                    feature2Description = featuresSection.Feature2Description,
                    feature2Icon = featuresSection.Feature2Icon,
                    feature2Link = featuresSection.Feature2Link,
                    feature3Title = featuresSection.Feature3Title,
                    feature3Subtitle = featuresSection.Feature3Subtitle,
                    feature3Description = featuresSection.Feature3Description,
                    feature3Icon = featuresSection.Feature3Icon,
                    feature3Link = featuresSection.Feature3Link,
                    isActive = featuresSection.IsActive,
                    displayOrder = featuresSection.DisplayOrder,
                    updatedAt = featuresSection.UpdatedAt
                };

                return Json(new { 
                    success = true, 
                    message = "Features section retrieved successfully.",
                    data = featuresData 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving features section: {Message}", ex.Message);
                return Json(new { 
                    success = false, 
                    message = $"An error occurred while retrieving the features section: {ex.Message}",
                    data = (object)null 
                });
            }
        }

        // GET: Admin/GetFeaturesTemplates
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFeaturesTemplates()
        {
            try
            {
                // Get all templates and handle null UpdatedAt values
                var templates = await _context.FeaturesTemplates
                    .Select(t => new
                    {
                        t.Id,
                        t.Nickname,
                        t.SectionTitle,
                        t.SectionSubtitle,
                        t.SectionDescription,
                        t.Feature1Title,
                        t.Feature1Subtitle,
                        t.Feature1Description,
                        t.Feature1Icon,
                        t.Feature1Link,
                        t.Feature2Title,
                        t.Feature2Subtitle,
                        t.Feature2Description,
                        t.Feature2Icon,
                        t.Feature2Link,
                        t.Feature3Title,
                        t.Feature3Subtitle,
                        t.Feature3Description,
                        t.Feature3Icon,
                        t.Feature3Link,
                        t.CreatedAt,
                        t.UpdatedAt,
                        LastModified = t.UpdatedAt ?? t.CreatedAt
                    })
                    .OrderByDescending(t => t.LastModified)
                    .ToListAsync();
                
                return Json(new { success = true, data = templates });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving features templates");
                
                // Return empty array instead of error if table doesn't exist
                if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("doesn't exist"))
                {
                    return Json(new { success = true, data = new List<object>() });
                }
                
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/SaveFeaturesTemplate
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveFeaturesTemplate([FromForm] FeaturesTemplateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                FeaturesTemplate template;
                if (model.Id == null || model.Id == 0)
                {
                    // Create new template
                    template = new FeaturesTemplate
                    {
                        Nickname = model.Nickname ?? "New Template",
                        SectionTitle = model.SectionTitle ?? "Key Skills & Technologies",
                        SectionSubtitle = model.SectionSubtitle ?? "Explore my expertise across different domains",
                        Feature1Title = model.Feature1Title ?? "Frontend Development",
                        Feature1Subtitle = model.Feature1Subtitle ?? "React, JavaScript, HTML5, CSS3, Bootstrap",
                        Feature1Description = model.Feature1Description ?? "Building responsive and interactive user interfaces with modern frameworks and best practices.",
                        Feature1Icon = model.Feature1Icon ?? "fas fa-code",
                        Feature1Link = model.Feature1Link ?? "/projects?category=frontend",
                        Feature2Title = model.Feature2Title ?? "Backend Development",
                        Feature2Subtitle = model.Feature2Subtitle ?? ".NET Core, C#, RESTful APIs, SQL Server",
                        Feature2Description = model.Feature2Description ?? "Creating robust server-side applications and APIs with enterprise-grade technologies.",
                        Feature2Icon = model.Feature2Icon ?? "fas fa-server",
                        Feature2Link = model.Feature2Link ?? "/projects?category=backend",
                        Feature3Title = model.Feature3Title ?? "Design & Tools",
                        Feature3Subtitle = model.Feature3Subtitle ?? "Adobe Creative Suite, UI/UX Design, Git, Docker",
                        Feature3Description = model.Feature3Description ?? "Crafting beautiful designs and managing development workflows with professional tools.",
                        Feature3Icon = model.Feature3Icon ?? "fas fa-palette",
                        Feature3Link = model.Feature3Link ?? "/projects?category=design",
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.FeaturesTemplates.Add(template);
                }
                else
                {
                    // Update existing template
                    template = await _context.FeaturesTemplates.FindAsync(model.Id);
                    if (template == null)
                    {
                        return Json(new { success = false, message = "Template not found" });
                    }

                    template.Nickname = model.Nickname ?? template.Nickname;
                    template.SectionTitle = model.SectionTitle ?? template.SectionTitle;
                    template.SectionSubtitle = model.SectionSubtitle ?? template.SectionSubtitle;
                    template.Feature1Title = model.Feature1Title ?? template.Feature1Title;
                    template.Feature1Subtitle = model.Feature1Subtitle ?? template.Feature1Subtitle;
                    template.Feature1Description = model.Feature1Description ?? template.Feature1Description;
                    template.Feature1Icon = model.Feature1Icon ?? template.Feature1Icon;
                    template.Feature1Link = model.Feature1Link ?? template.Feature1Link;
                    template.Feature2Title = model.Feature2Title ?? template.Feature2Title;
                    template.Feature2Subtitle = model.Feature2Subtitle ?? template.Feature2Subtitle;
                    template.Feature2Description = model.Feature2Description ?? template.Feature2Description;
                    template.Feature2Icon = model.Feature2Icon ?? template.Feature2Icon;
                    template.Feature2Link = model.Feature2Link ?? template.Feature2Link;
                    template.Feature3Title = model.Feature3Title ?? template.Feature3Title;
                    template.Feature3Subtitle = model.Feature3Subtitle ?? template.Feature3Subtitle;
                    template.Feature3Description = model.Feature3Description ?? template.Feature3Description;
                    template.Feature3Icon = model.Feature3Icon ?? template.Feature3Icon;
                    template.Feature3Link = model.Feature3Link ?? template.Feature3Link;
                    template.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, data = template });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving features template");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // DELETE: Admin/DeleteFeaturesTemplate
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeaturesTemplate(int id)
        {
            try
            {
                var template = await _context.FeaturesTemplates.FindAsync(id);
                if (template == null)
                {
                    return Json(new { success = false, message = "Template not found" });
                }

                _context.FeaturesTemplates.Remove(template);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Features template deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting features template");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class LoginModel
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        public class HeroSectionModel
        {
            public string? HeaderTitle { get; set; }
            public string? HeaderSubtitle { get; set; }
            public string? HeaderDescription { get; set; }
            public string? HeaderBackgroundImageUrl { get; set; }
            public string? HeaderBackgroundVideoUrl { get; set; }
            public string? HeaderPrimaryButtonText { get; set; }
            public string? HeaderPrimaryButtonUrl { get; set; }
            public string? ImageOverlayColor { get; set; }
            public int ImageOverlayOpacity { get; set; } = 50;
            public string? VideoOverlayColor { get; set; }
            public int VideoOverlayOpacity { get; set; } = 50;
        }

        public class HeroTemplateModel
        {
            public int? Id { get; set; }
            public string? Nickname { get; set; }
            public string? HeaderTitle { get; set; }
            public string? HeaderSubtitle { get; set; }
            public string? HeaderDescription { get; set; }
            public string? HeaderBackgroundImageUrl { get; set; }
            public string? HeaderBackgroundVideoUrl { get; set; }
            public string? HeaderPrimaryButtonText { get; set; }
            public string? HeaderPrimaryButtonUrl { get; set; }
            public string? ImageOverlayColor { get; set; }
            public int ImageOverlayOpacity { get; set; } = 50;
            public string? VideoOverlayColor { get; set; }
            public int VideoOverlayOpacity { get; set; } = 50;
        }

        public class FeaturesSectionModel
        {
            public string? SectionTitle { get; set; }
            public string? SectionSubtitle { get; set; }
            public string? Feature1Title { get; set; }
            public string? Feature1Subtitle { get; set; }
            public string? Feature1Description { get; set; }
            public string? Feature1Icon { get; set; }
            public string? Feature1Link { get; set; }
            public string? Feature2Title { get; set; }
            public string? Feature2Subtitle { get; set; }
            public string? Feature2Description { get; set; }
            public string? Feature2Icon { get; set; }
            public string? Feature2Link { get; set; }
            public string? Feature3Title { get; set; }
            public string? Feature3Subtitle { get; set; }
            public string? Feature3Description { get; set; }
            public string? Feature3Icon { get; set; }
            public string? Feature3Link { get; set; }
            public bool IsActive { get; set; } = true;
            public int DisplayOrder { get; set; } = 1;
        }

        public class FeaturesTemplateModel
        {
            public int? Id { get; set; }
            public string? Nickname { get; set; }
            public string? SectionTitle { get; set; }
            public string? SectionSubtitle { get; set; }
            public string? Feature1Title { get; set; }
            public string? Feature1Subtitle { get; set; }
            public string? Feature1Description { get; set; }
            public string? Feature1Icon { get; set; }
            public string? Feature1Link { get; set; }
            public string? Feature2Title { get; set; }
            public string? Feature2Subtitle { get; set; }
            public string? Feature2Description { get; set; }
            public string? Feature2Icon { get; set; }
            public string? Feature2Link { get; set; }
            public string? Feature3Title { get; set; }
            public string? Feature3Subtitle { get; set; }
            public string? Feature3Description { get; set; }
            public string? Feature3Icon { get; set; }
            public string? Feature3Link { get; set; }
        }
    }
}