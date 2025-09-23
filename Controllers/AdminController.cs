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
using BCrypt.Net;

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
            return BCrypt.Net.BCrypt.HashPassword(password, 12); // Use work factor of 12 for security
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private bool IsReadOnlyUser()
        {
            return User.IsInRole("ReadOnly");
        }

        private IActionResult ReadOnlyError()
        {
            ViewBag.ErrorMessage = "This guest user account is read only";
            return View("Dashboard"); // Return to dashboard with error message
        }

        // Custom authorization check for POST actions
        private bool CheckWritePermission()
        {
            if (User.IsInRole("ReadOnly"))
            {
                return false;
            }
            return true;
        }

        [AllowAnonymous]
        public IActionResult Login(string action = null)
        {
            try
            {
                _logger.LogInformation("Login page requested");
                
                if (User.Identity?.IsAuthenticated == true)
                {
                    _logger.LogInformation($"User already authenticated as: {User.Identity.Name}");
                    
                    // If they explicitly want to logout and login as someone else
                    if (action == "switch")
                    {
                        ViewBag.ShowSwitchMessage = true;
                        ViewBag.CurrentUser = User.Identity.Name;
                        ViewBag.ErrorMessage = null;
                        return View();
                    }
                    
                    // Otherwise redirect to dashboard
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
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Console.WriteLine($"🔓 User {User.Identity?.Name} logging out");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Console.WriteLine("✅ Logout successful");
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Logout error: {ex.Message}");
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            try
            {
                Console.WriteLine("=== LOGIN POST REQUEST RECEIVED ===");
                Console.WriteLine($"Model is null: {model == null}");
                Console.WriteLine($"Username: '{model?.Username}'");
                Console.WriteLine($"Password length: {model?.Password?.Length ?? 0}");
                Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
                Console.WriteLine($"Current user authenticated: {User.Identity?.IsAuthenticated}");
                Console.WriteLine($"Current user name: '{User.Identity?.Name}'");
                
                // Allow concurrent sessions - don't force logout
                
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("❌ ModelState validation failed:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"  - {error.ErrorMessage}");
                    }
                    ViewBag.ErrorMessage = "Please provide valid username and password.";
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    ViewBag.ErrorMessage = "Username and password are required.";
                    return View(model);
                }

                // Optimized query - only select the fields we need
                var admin = await _context.Admins
                    .Where(a => a.Username == model.Username)
                    .Select(a => new { a.Username, a.PasswordHash, a.Role })
                    .FirstOrDefaultAsync();

                if (admin == null || !VerifyPassword(model.Password, admin.PasswordHash))
                {
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.NameIdentifier, admin.Username),
                    new Claim(ClaimTypes.Role, admin.Role)
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
                Console.WriteLine($"❌ LOGIN ERROR: {ex.GetType().Name}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                _logger.LogError(ex, "Error during login");
                ViewBag.ErrorMessage = $"An error occurred during login: {ex.Message}";
                return View(model);
            }
        }

        [Authorize(Roles = "Admin,ReadOnly")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var homePage = await _homePageService.GetHomePageAsync();
                ViewBag.HomePage = homePage;
                
                // Pass user role to view
                ViewBag.UserRole = User.IsInRole("ReadOnly") ? "ReadOnly" : "Admin";
                ViewBag.IsReadOnly = User.IsInRole("ReadOnly");
                
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
                    Nickname = model.Nickname ?? "",
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
                existingTemplate.Nickname = model.Nickname ?? "";
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
                        data = (object?)null 
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
                    data = (object?)null 
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
                        data = (object?)null 
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
                    data = (object?)null 
                });
            }
        }

        // GET: Admin/GetFeaturesTemplate
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetFeaturesTemplate")]
        public async Task<IActionResult> GetFeaturesTemplate(int id)
        {
            try
            {
                Console.WriteLine($"GetFeaturesTemplate: Loading template with ID {id}");
                
                // First check if the table exists and has data
                var templateCount = await _context.FeaturesTemplates.CountAsync();
                Console.WriteLine($"GetFeaturesTemplate: Total templates in database: {templateCount}");
                
                // Get all template IDs for debugging
                var allTemplateIds = await _context.FeaturesTemplates.Select(t => t.Id).ToListAsync();
                Console.WriteLine($"GetFeaturesTemplate: Available template IDs: [{string.Join(", ", allTemplateIds)}]");
                
                var template = await _context.FeaturesTemplates.FindAsync(id);
                
                if (template == null)
                {
                    Console.WriteLine($"GetFeaturesTemplate: Template with ID {id} not found");
                    return Json(new { 
                        success = false, 
                        message = $"Template with ID {id} not found. Available IDs: [{string.Join(", ", allTemplateIds)}]",
                        data = (object?)null 
                    });
                }
                
                Console.WriteLine($"GetFeaturesTemplate: Found template - ID={template.Id}, Nickname={template.Nickname}");

                // Create properly formatted template data like GetHeroTemplate does
                var templateData = new
                {
                    id = template.Id,
                    nickname = template.Nickname ?? "",
                    sectionTitle = template.SectionTitle ?? "",
                    sectionSubtitle = template.SectionSubtitle ?? "",
                    sectionDescription = template.SectionDescription ?? "",
                    feature1Title = template.Feature1Title ?? "",
                    feature1Subtitle = template.Feature1Subtitle ?? "",
                    feature1Description = template.Feature1Description ?? "",
                    feature1Icon = template.Feature1Icon ?? "",
                    feature1Link = template.Feature1Link ?? "",
                    feature2Title = template.Feature2Title ?? "",
                    feature2Subtitle = template.Feature2Subtitle ?? "",
                    feature2Description = template.Feature2Description ?? "",
                    feature2Icon = template.Feature2Icon ?? "",
                    feature2Link = template.Feature2Link ?? "",
                    feature3Title = template.Feature3Title ?? "",
                    feature3Subtitle = template.Feature3Subtitle ?? "",
                    feature3Description = template.Feature3Description ?? "",
                    feature3Icon = template.Feature3Icon ?? "",
                    feature3Link = template.Feature3Link ?? "",
                    createdAt = template.CreatedAt,
                    updatedAt = template.UpdatedAt
                };
                
                Console.WriteLine($"GetFeaturesTemplate: Returning template data for {template.Nickname}");
                
                return Json(new { 
                    success = true, 
                    message = "Features template retrieved successfully.",
                    data = templateData 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetFeaturesTemplate: Error - {ex.Message}");
                Console.WriteLine($"GetFeaturesTemplate: Stack trace - {ex.StackTrace}");
                _logger.LogError(ex, "Error retrieving features template");
                return Json(new { 
                    success = false, 
                    message = $"An error occurred while retrieving the features template: {ex.Message}",
                    data = (object?)null 
                });
            }
        }

        // GET: Admin/GetFeaturesTemplates
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetFeaturesTemplates")]
        public async Task<IActionResult> GetFeaturesTemplates()
        {
            try
            {
                Console.WriteLine("GetFeaturesTemplates: Starting query...");
                
                // Use the same approach as HeroTemplates - simple and direct
                var templateCount = await _context.FeaturesTemplates.CountAsync();
                Console.WriteLine($"GetFeaturesTemplates: Found {templateCount} templates in database");
                
                var templates = await _context.FeaturesTemplates
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => new
                    {
                        id = t.Id,
                        nickname = t.Nickname,
                        sectionTitle = t.SectionTitle,
                        sectionSubtitle = t.SectionSubtitle,
                        createdAt = t.CreatedAt,
                        updatedAt = t.UpdatedAt
                    })
                    .ToListAsync();
                
                Console.WriteLine($"GetFeaturesTemplates: Processed {templates.Count} templates");
                
                return Json(new { 
                    success = true, 
                    message = "Features templates retrieved successfully.",
                    data = templates 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetFeaturesTemplates: Error - {ex.Message}");
                Console.WriteLine($"GetFeaturesTemplates: Stack trace - {ex.StackTrace}");
                _logger.LogError(ex, "Error retrieving features templates");
                
                // Return empty array instead of error if table doesn't exist
                if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("doesn't exist") || ex.Message.Contains("Invalid column name"))
                {
                    return Json(new { success = true, data = new List<object>() });
                }
                
                return Json(new { 
                    success = false, 
                    message = $"An error occurred while retrieving features templates: {ex.Message}",
                    data = new object[] { } 
                });
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

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.SectionTitle))
                {
                    return Json(new { success = false, message = "Section Title is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature1Title))
                {
                    return Json(new { success = false, message = "Feature 1 Title is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature1Subtitle))
                {
                    return Json(new { success = false, message = "Feature 1 Subtitle is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature1Link))
                {
                    return Json(new { success = false, message = "Feature 1 Link is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature2Title))
                {
                    return Json(new { success = false, message = "Feature 2 Title is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature2Subtitle))
                {
                    return Json(new { success = false, message = "Feature 2 Subtitle is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature2Link))
                {
                    return Json(new { success = false, message = "Feature 2 Link is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature3Title))
                {
                    return Json(new { success = false, message = "Feature 3 Title is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature3Subtitle))
                {
                    return Json(new { success = false, message = "Feature 3 Subtitle is required." });
                }
                if (string.IsNullOrWhiteSpace(model.Feature3Link))
                {
                    return Json(new { success = false, message = "Feature 3 Link is required." });
                }

                FeaturesTemplate template;
                if (model.Id == null || model.Id == 0)
                {
                    // Create new template
                    template = new FeaturesTemplate
                    {
                        Nickname = model.Nickname ?? "New Template",
                        SectionTitle = model.SectionTitle ?? "",
                        SectionSubtitle = model.SectionSubtitle ?? "", // Optional
                        Feature1Title = model.Feature1Title ?? "",
                        Feature1Subtitle = model.Feature1Subtitle ?? "",
                        Feature1Description = model.Feature1Description ?? "", // Optional
                        Feature1Icon = model.Feature1Icon ?? "", // Optional
                        Feature1Link = model.Feature1Link ?? "",
                        Feature2Title = model.Feature2Title ?? "",
                        Feature2Subtitle = model.Feature2Subtitle ?? "",
                        Feature2Description = model.Feature2Description ?? "", // Optional
                        Feature2Icon = model.Feature2Icon ?? "", // Optional
                        Feature2Link = model.Feature2Link ?? "",
                        Feature3Title = model.Feature3Title ?? "",
                        Feature3Subtitle = model.Feature3Subtitle ?? "",
                        Feature3Description = model.Feature3Description ?? "", // Optional
                        Feature3Icon = model.Feature3Icon ?? "", // Optional
                        Feature3Link = model.Feature3Link ?? "",
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
                    template.SectionSubtitle = model.SectionSubtitle ?? ""; // Optional
                    template.Feature1Title = model.Feature1Title ?? template.Feature1Title;
                    template.Feature1Subtitle = model.Feature1Subtitle ?? template.Feature1Subtitle;
                    template.Feature1Description = model.Feature1Description ?? ""; // Optional
                    template.Feature1Icon = model.Feature1Icon ?? ""; // Optional
                    template.Feature1Link = model.Feature1Link ?? template.Feature1Link;
                    template.Feature2Title = model.Feature2Title ?? template.Feature2Title;
                    template.Feature2Subtitle = model.Feature2Subtitle ?? template.Feature2Subtitle;
                    template.Feature2Description = model.Feature2Description ?? ""; // Optional
                    template.Feature2Icon = model.Feature2Icon ?? ""; // Optional
                    template.Feature2Link = model.Feature2Link ?? template.Feature2Link;
                    template.Feature3Title = model.Feature3Title ?? template.Feature3Title;
                    template.Feature3Subtitle = model.Feature3Subtitle ?? template.Feature3Subtitle;
                    template.Feature3Description = model.Feature3Description ?? ""; // Optional
                    template.Feature3Icon = model.Feature3Icon ?? ""; // Optional
                    template.Feature3Link = model.Feature3Link ?? template.Feature3Link;
                    template.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                
                // Return properly formatted data like GetFeaturesTemplate
                var templateData = new
                {
                    id = template.Id,
                    nickname = template.Nickname,
                    sectionTitle = template.SectionTitle,
                    sectionSubtitle = template.SectionSubtitle,
                    sectionDescription = template.SectionDescription,
                    feature1Title = template.Feature1Title,
                    feature1Subtitle = template.Feature1Subtitle,
                    feature1Description = template.Feature1Description,
                    feature1Icon = template.Feature1Icon,
                    feature1Link = template.Feature1Link,
                    feature2Title = template.Feature2Title,
                    feature2Subtitle = template.Feature2Subtitle,
                    feature2Description = template.Feature2Description,
                    feature2Icon = template.Feature2Icon,
                    feature2Link = template.Feature2Link,
                    feature3Title = template.Feature3Title,
                    feature3Subtitle = template.Feature3Subtitle,
                    feature3Description = template.Feature3Description,
                    feature3Icon = template.Feature3Icon,
                    feature3Link = template.Feature3Link,
                    createdAt = template.CreatedAt,
                    updatedAt = template.UpdatedAt
                };
                
                return Json(new { 
                    success = true, 
                    message = $"Features template '{template.Nickname}' saved successfully!",
                    data = templateData 
                });
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

        // CTA Section Methods - Following same pattern as Hero and Features
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCTASection([FromForm] CTASection model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Get existing CTA section or create new one
                var ctaSection = await _context.CTASections.FirstOrDefaultAsync();
                if (ctaSection == null)
                {
                    ctaSection = new CTASection
                    {
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.CTASections.Add(ctaSection);
                }

                // Update CTA section properties
                ctaSection.Title = model.Title ?? "";
                ctaSection.Subtitle = model.Subtitle ?? "";
                ctaSection.ButtonText = model.ButtonText ?? "";
                ctaSection.ButtonLink = model.ButtonLink ?? "";
                ctaSection.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Notify WebSocket clients
                await _webSocketService.BroadcastMessageAsync("{\"type\":\"ctaDataUpdated\",\"timestamp\":\"" + DateTime.UtcNow.ToString("O") + "\"}");

                return Json(new { success = true, message = "CTA section saved successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving CTA section");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCTASection()
        {
            try
            {
                var ctaSection = await _context.CTASections.FirstOrDefaultAsync();
                
                if (ctaSection == null)
                {
                    return Json(new { success = false, message = "No CTA section found." });
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        title = ctaSection.Title ?? "",
                        subtitle = ctaSection.Subtitle ?? "",
                        buttonText = ctaSection.ButtonText ?? "",
                        buttonLink = ctaSection.ButtonLink ?? "",
                        lastModified = ctaSection.UpdatedAt ?? ctaSection.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting CTA section");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetCTATemplate")]
        public async Task<IActionResult> GetCTATemplate(int id)
        {
            try
            {
                var template = await _context.CTATemplates.FindAsync(id);
                
                if (template == null)
                {
                    return Json(new { success = false, message = "CTA template not found." });
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = template.Id,
                        nickname = template.Nickname ?? "",
                        title = template.Title ?? "",
                        subtitle = template.Subtitle ?? "",
                        buttonText = template.ButtonText ?? "",
                        buttonLink = template.ButtonLink ?? "",
                        lastModified = template.UpdatedAt.GetValueOrDefault(template.CreatedAt)
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting CTA template: {Error}", ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetCTATemplates")]
        public async Task<IActionResult> GetCTATemplates()
        {
            try
            {
                Console.WriteLine("GetCTATemplates: Starting query...");
                
                // Count templates for logging/debugging
                var templateCount = await _context.CTATemplates.CountAsync();
                Console.WriteLine($"GetCTATemplates: Found {templateCount} templates in database");
                
                // Query and project templates
                var templates = await _context.CTATemplates
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => new
                    {
                        id = t.Id,
                        nickname = t.Nickname ?? "",
                        lastModified = t.UpdatedAt ?? t.CreatedAt
                    })
                    .ToListAsync();
                
                Console.WriteLine($"GetCTATemplates: Processed {templates.Count} templates");
                foreach (var template in templates)
                {
                    Console.WriteLine($"Template: Id={template.id}, Nickname={template.nickname}, LastModified={template.lastModified}");
                }
                
                return Json(new { 
                    success = true, 
                    message = "CTA templates retrieved successfully.",
                    data = templates 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCTATemplates: Error - {ex.Message}");
                Console.WriteLine($"GetCTATemplates: Stack trace - {ex.StackTrace}");
                _logger.LogError(ex, "Error retrieving CTA templates");
                
                // Return empty array instead of error if table doesn't exist
                if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("doesn't exist") || ex.Message.Contains("Invalid column name"))
                {
                    return Json(new { success = true, data = new List<object>() });
                }
                
                return Json(new { 
                    success = false, 
                    message = $"An error occurred while retrieving CTA templates: {ex.Message}",
                    data = new object[] { } 
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCTATemplate([FromForm] CTATemplate model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    return Json(new { success = false, message = "Title is required." });
                }

                if (string.IsNullOrWhiteSpace(model.Subtitle))
                {
                    return Json(new { success = false, message = "Subtitle is required." });
                }

                if (string.IsNullOrWhiteSpace(model.ButtonText))
                {
                    return Json(new { success = false, message = "Button text is required." });
                }

                if (string.IsNullOrWhiteSpace(model.ButtonLink))
                {
                    return Json(new { success = false, message = "Button link is required." });
                }

                // Check if template with this nickname already exists
                var existingTemplate = await _context.CTATemplates
                    .FirstOrDefaultAsync(t => t.Nickname == model.Nickname);
                
                if (existingTemplate != null)
                {
                    return Json(new { success = false, message = "A template with this nickname already exists. Please choose a different name." });
                }

                // Create new CTA template
                var template = new CTATemplate
                {
                    Nickname = model.Nickname ?? "",
                    Title = model.Title ?? "",
                    Subtitle = model.Subtitle ?? "",
                    ButtonText = model.ButtonText ?? "",
                    ButtonLink = model.ButtonLink ?? "",
                    CreatedAt = DateTime.UtcNow
                };

                _context.CTATemplates.Add(template);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "CTA template saved successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving CTA template");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCTATemplate([FromForm] CTATemplate model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    return Json(new { success = false, message = "Title is required." });
                }

                if (string.IsNullOrWhiteSpace(model.Subtitle))
                {
                    return Json(new { success = false, message = "Subtitle is required." });
                }

                if (string.IsNullOrWhiteSpace(model.ButtonText))
                {
                    return Json(new { success = false, message = "Button text is required." });
                }

                if (string.IsNullOrWhiteSpace(model.ButtonLink))
                {
                    return Json(new { success = false, message = "Button link is required." });
                }

                // Find existing template by ID
                var existingTemplate = await _context.CTATemplates.FindAsync(model.Id);
                
                if (existingTemplate == null)
                {
                    return Json(new { success = false, message = "Template not found." });
                }

                // Update template properties
                existingTemplate.Nickname = model.Nickname ?? "";
                existingTemplate.Title = model.Title ?? "";
                existingTemplate.Subtitle = model.Subtitle ?? "";
                existingTemplate.ButtonText = model.ButtonText ?? "";
                existingTemplate.ButtonLink = model.ButtonLink ?? "";
                existingTemplate.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "CTA template updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating CTA template");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCTATemplate(int id)
        {
            try
            {
                var template = await _context.CTATemplates.FindAsync(id);
                if (template == null)
                {
                    return Json(new { success = false, message = "Template not found" });
                }

                _context.CTATemplates.Remove(template);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "CTA template deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting CTA template");
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