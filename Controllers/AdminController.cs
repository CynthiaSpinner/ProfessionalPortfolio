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

        public AdminController(PortfolioContext context, ILogger<AdminController> logger, IConfiguration configuration, IHomePageService homePageService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _homePageService = homePageService;
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
                ViewBag.ErrorMessage = "An error occurred during login. Please try again.";
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
                    _logger.LogWarning($"Hero section validation errors: {string.Join(", ", errors)}");
                    return Json(new { success = false, message = "Validation failed", errors = errors });
                }

                // Get existing homepage or create new one
                var existingHomePage = await _homePageService.GetHomePageAsync();
                if (existingHomePage == null)
                {
                    existingHomePage = new HomePage
                    {
                        IsActive = true,
                        DisplayOrder = 1,
                        CreatedAt = DateTime.UtcNow
                    };
                }

                // Update hero section properties
                existingHomePage.HeaderTitle = model.HeaderTitle ?? string.Empty;
                existingHomePage.HeaderSubtitle = model.HeaderSubtitle ?? string.Empty;
                existingHomePage.HeaderDescription = model.HeaderDescription ?? string.Empty;
                existingHomePage.HeaderBackgroundImageUrl = model.HeaderBackgroundImageUrl ?? string.Empty;
                existingHomePage.HeaderBackgroundVideoUrl = model.HeaderBackgroundVideoUrl ?? string.Empty;
                existingHomePage.HeaderPrimaryButtonText = model.HeaderPrimaryButtonText ?? string.Empty;
                existingHomePage.HeaderPrimaryButtonUrl = model.HeaderPrimaryButtonUrl ?? string.Empty;
                existingHomePage.HeaderOverlayColor = model.ImageOverlayColor ?? "#000000";
                existingHomePage.HeaderOverlayOpacity = model.ImageOverlayOpacity / 100f; // Convert percentage to decimal
                existingHomePage.UpdatedAt = DateTime.UtcNow;

                // Save to database
                var savedHomePage = await _homePageService.UpdateHeaderSectionAsync(existingHomePage);

                _logger.LogInformation($"Hero section saved successfully. HomePage ID: {savedHomePage.Id}");

                return Json(new { 
                    success = true, 
                    message = "Hero section saved successfully!",
                    homePageId = savedHomePage.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving hero section");
                return Json(new { 
                    success = false, 
                    message = "An error occurred while saving the hero section. Please try again.",
                    error = ex.Message
                });
            }
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
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
    }
}