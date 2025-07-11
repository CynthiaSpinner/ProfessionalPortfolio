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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
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
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning($"Model validation errors: {string.Join(", ", errors)}");
                    ViewBag.ErrorMessage = "Please provide valid username and password.";
                    return View(model);
                }

                _logger.LogInformation($"login attempt for username: {model.Username}");

                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    _logger.LogWarning("login attempt with empty username or password");
                    ViewBag.ErrorMessage = "Username and password are required.";
                    return View(model);
                }

                var hashedPassword = HashPassword(model.Password);
                _logger.LogInformation($"Input password hash: {hashedPassword}");

                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Username == model.Username);

                if (admin == null)
                {
                    _logger.LogWarning($"No admin found with username: {model.Username}");
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }

                _logger.LogInformation($"Stored password hash: {admin.PasswordHash}");
                _logger.LogInformation($"Password match: {admin.PasswordHash == hashedPassword}");

                if (admin.PasswordHash != hashedPassword)
                {
                    _logger.LogWarning($"Password mismatch for username: {model.Username}");
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }

                _logger.LogInformation($"login successful for username: {model.Username}");
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
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                // Test basic connection
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    return Json(new { 
                        success = false, 
                        message = "Cannot connect to database",
                        details = "Database connection failed"
                    });
                }

                // Test a simple query
                var adminCount = await _context.Admins.CountAsync();
                
                return Json(new { 
                    success = true, 
                    message = "Database connection successful",
                    details = $"Connected successfully. Found {adminCount} admin users.",
                    adminCount = adminCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = "Database connection error",
                    details = ex.Message,
                    exceptionType = ex.GetType().Name
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
                return Json(new { success = false, message = "An error occurred while saving the hero section." });
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
                _logger.LogError(ex, "Error retrieving hero templates");
                return Json(new { 
                    success = false, 
                    message = "An error occurred while retrieving hero templates.",
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
                var template = await _context.HeroTemplates.FindAsync(id);
                
                if (template == null)
                {
                    return Json(new { 
                        success = false, 
                        message = "Template not found.",
                        data = (object)null 
                    });
                }

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
                _logger.LogError(ex, "Error retrieving hero template");
                return Json(new { 
                    success = false, 
                    message = "An error occurred while retrieving the hero template.",
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
    }
}