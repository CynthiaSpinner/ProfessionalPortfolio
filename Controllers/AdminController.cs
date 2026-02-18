using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services;
using Portfolio.Services.DTOs;
using Portfolio.Data.Repositories;
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
        private readonly ISiteSettingsService _siteSettingsService;
        private readonly ISkillsCategoryService _skillsCategoryService;
        private readonly IFeaturesSectionRepository _featuresSectionRepository;
        private readonly IFeaturesSectionService _featuresSectionService;
        private readonly ICTASectionRepository _ctaSectionRepository;
        private readonly ICTASectionService _ctaSectionService;
        private readonly IProjectService _projectService;

        public AdminController(
            PortfolioContext context,
            ILogger<AdminController> logger,
            IConfiguration configuration,
            IHomePageService homePageService,
            WebSocketService webSocketService,
            ISiteSettingsService siteSettingsService,
            ISkillsCategoryService skillsCategoryService,
            IFeaturesSectionRepository featuresSectionRepository,
            IFeaturesSectionService featuresSectionService,
            ICTASectionRepository ctaSectionRepository,
            ICTASectionService ctaSectionService,
            IProjectService projectService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _homePageService = homePageService;
            _webSocketService = webSocketService;
            _siteSettingsService = siteSettingsService;
            _skillsCategoryService = skillsCategoryService;
            _featuresSectionRepository = featuresSectionRepository;
            _featuresSectionService = featuresSectionService;
            _ctaSectionRepository = ctaSectionRepository;
            _ctaSectionService = ctaSectionService;
            _projectService = projectService;
        }

        private static bool IsBcryptHash(string hash)
        {
            return !string.IsNullOrEmpty(hash) && (hash.StartsWith("$2a$", StringComparison.Ordinal) || hash.StartsWith("$2b$", StringComparison.Ordinal) || hash.StartsWith("$2y$", StringComparison.Ordinal));
        }

        private bool VerifyPassword(string plainPassword, string storedHash)
        {
            if (IsBcryptHash(storedHash))
                return BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);
            var sha256Hash = HashPasswordLegacy(plainPassword);
            return sha256Hash == storedHash;
        }

        private static string HashPasswordLegacy(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
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

                var admin = await _context.Admins
                    .Where(a => a.Username == model.Username)
                    .Select(a => new { a.Username, a.PasswordHash, a.Role })
                    .FirstOrDefaultAsync();

                if (admin == null || !VerifyPassword(model.Password, admin.PasswordHash))
                {
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }

                var role = admin.Role ?? "Admin";
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, role)
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

        /// <summary>
        /// One-time diagnostic: check DB connection and schema. GET /Admin/CheckDb?secret=your_ADMIN_RESET_SECRET
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckDb([FromQuery] string? secret)
        {
            var expectedSecret = _configuration["ADMIN_RESET_SECRET"] ?? Environment.GetEnvironmentVariable("ADMIN_RESET_SECRET");
            if (string.IsNullOrEmpty(expectedSecret) || secret != expectedSecret)
            {
                return Unauthorized();
            }

            var connectionOk = false;
            string? connectionError = null;
            var admins = new List<object>();
            List<string>? tables = null;
            object? schemaCheck = null;
            string? providerName = null;

            try
            {
                await _context.Database.CanConnectAsync();
                connectionOk = true;
                var conn = _context.Database.GetDbConnection();
                providerName = conn.GetType().Name;
            }
            catch (Exception ex)
            {
                connectionError = ex.Message;
                return Ok(new { connectionOk = false, connectionError, admins, tables, schemaCheck, providerName });
            }

            try
            {
                var list = await _context.Admins
                    .Select(a => new { a.Username, a.PasswordHash })
                    .ToListAsync();
                admins = list.Cast<object>().ToList();
            }
            catch (Exception ex)
            {
                connectionError = $"Query failed: {ex.Message}";
            }

            // Schema check: list tables and HomePages columns (to verify hero image columns exist)
            try
            {
                await _context.Database.OpenConnectionAsync();
                var conn = _context.Database.GetDbConnection();
                var isPostgres = conn.GetType().Name.Contains("Npgsql", StringComparison.OrdinalIgnoreCase);

                var expectedHeroColumns = new[] { "HeaderBackgroundImageData", "HeaderBackgroundImageContentType" };

                if (isPostgres)
                {
                    // Postgres: information_schema
                    var tableList = new List<string>();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE' ORDER BY table_name";
                        using var r = await cmd.ExecuteReaderAsync();
                        while (await r.ReadAsync()) tableList.Add(r.GetString(0));
                    }
                    tables = tableList;

                    var homePageColumnNames = new List<string>();
                    var homePageColumnsDetail = new List<object>();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'HomePages' ORDER BY ordinal_position";
                        using var r = await cmd.ExecuteReaderAsync();
                        while (await r.ReadAsync())
                        {
                            var col = r.GetString(0);
                            var typ = r.GetString(1);
                            homePageColumnNames.Add(col);
                            homePageColumnsDetail.Add(new { column = col, type = typ });
                        }
                    }
                    var heroColumnsOk = expectedHeroColumns.All(c => homePageColumnNames.Contains(c, StringComparer.OrdinalIgnoreCase));
                    schemaCheck = new
                    {
                        table = "HomePages",
                        columns = homePageColumnsDetail,
                        expectedHeroColumns = expectedHeroColumns,
                        heroColumnsOk,
                        message = heroColumnsOk ? "HomePages has required hero image columns." : "Missing one or more hero columns. On Postgres run add-hero-image-columns.sql once."
                    };
                }
                else
                {
                    // SQL Server: INFORMATION_SCHEMA
                    var tableList = new List<string>();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
                        using var r = await cmd.ExecuteReaderAsync();
                        while (await r.ReadAsync()) tableList.Add(r.GetString(0));
                    }
                    tables = tableList;

                    var homePageColumnNames = new List<string>();
                    var homePageColumnsDetail = new List<object>();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HomePages' ORDER BY ORDINAL_POSITION";
                        using var r = await cmd.ExecuteReaderAsync();
                        while (await r.ReadAsync())
                        {
                            var col = r.GetString(0);
                            var typ = r.GetString(1);
                            homePageColumnNames.Add(col);
                            homePageColumnsDetail.Add(new { column = col, type = typ });
                        }
                    }
                    var heroColumnsOk = expectedHeroColumns.All(c => homePageColumnNames.Contains(c, StringComparer.OrdinalIgnoreCase));
                    schemaCheck = new
                    {
                        table = "HomePages",
                        columns = homePageColumnsDetail,
                        expectedHeroColumns = expectedHeroColumns,
                        heroColumnsOk,
                        message = heroColumnsOk ? "HomePages has required hero image columns." : "Missing one or more hero columns (run migrations for SQL Server)."
                    };
                }
            }
            catch (Exception ex)
            {
                schemaCheck = new { error = ex.Message, hint = "Could not read schema. Check DB user has permission to read information_schema." };
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return Ok(new { connectionOk, connectionError, admins, tables, schemaCheck, providerName });
        }

        /// <summary>
        /// One-time: sync an admin's password hash in the DB. POST body: secret, username, newPassword
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SyncAdminPassword([FromForm] string? secret, [FromForm] string? username, [FromForm] string? newPassword)
        {
            var expectedSecret = _configuration["ADMIN_RESET_SECRET"] ?? Environment.GetEnvironmentVariable("ADMIN_RESET_SECRET");
            if (string.IsNullOrEmpty(expectedSecret) || secret != expectedSecret || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword))
            {
                return Unauthorized();
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
            if (admin == null)
            {
                return NotFound("Admin user not found.");
            }

            admin.PasswordHash = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Password hash updated for admin {Username}", username);
            return Ok("Password updated.");
        }

        [Authorize(Roles = "Admin,ReadOnly")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var homePage = await _homePageService.GetHomePageAsync();
                ViewBag.HomePage = homePage;
                SiteSettings navSettings;
                try { navSettings = await _siteSettingsService.GetOrCreateAsync(); } catch { navSettings = new SiteSettings(); }
                ViewBag.NavSettings = navSettings;
                List<SkillsCategory> skillsCategories;
                try { skillsCategories = await _skillsCategoryService.GetOrderedAsync(); } catch { skillsCategories = new List<SkillsCategory>(); }
                ViewBag.SkillsCategories = skillsCategories;
                ViewBag.SkillsTeaserLinkText = skillsCategories.FirstOrDefault()?.TeaserLinkText;
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

        [Authorize(Roles = "Admin,ReadOnly")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHeroSection([FromForm] HeroSectionModel model, [FromForm] IFormFile? HeroBackgroundImage)
        {
            try
            {
                _logger.LogInformation("[Hero] SaveHeroSection called. HeroBackgroundImage: {HasFile}, Length: {Length}, ContentType: {ContentType}, FileName: {FileName}",
                    HeroBackgroundImage != null, HeroBackgroundImage?.Length ?? 0, HeroBackgroundImage?.ContentType ?? "(null)", HeroBackgroundImage?.FileName ?? "(null)");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning("[Hero] ModelState invalid: {Errors}", string.Join(", ", errors));
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // Single database query - get or create home page
                var homePage = await _homePageService.GetHomePageAsync();
                
                if (homePage == null)
                {
                    homePage = new HomePage();
                    _context.HomePages.Add(homePage);
                    _logger.LogInformation("[Hero] Created new HomePage entity.");
                }

                // Update hero section properties
                homePage.HeaderTitle = model.HeaderTitle ?? "Welcome to My Portfolio";
                homePage.HeaderSubtitle = model.HeaderSubtitle ?? "I am a passionate software engineer specializing in full-stack development.";
                homePage.HeaderDescription = model.HeaderDescription ?? "";
                homePage.HeaderBackgroundVideoUrl = model.HeaderBackgroundVideoUrl ?? "";
                homePage.HeaderPrimaryButtonText = model.HeaderPrimaryButtonText ?? "View Projects";
                homePage.HeaderPrimaryButtonUrl = model.HeaderPrimaryButtonUrl ?? "/projects";
                homePage.HeaderOverlayColor = model.ImageOverlayColor ?? "#000000";
                homePage.HeaderOverlayOpacity = model.ImageOverlayOpacity / 100f;
                homePage.UpdatedAt = DateTime.UtcNow;

                // Hero background image: upload file (store in DB) or use URL
                if (HeroBackgroundImage != null && HeroBackgroundImage.Length > 0)
                {
                    var allowed = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
                    var contentType = HeroBackgroundImage.ContentType?.ToLowerInvariant() ?? "";
                    _logger.LogInformation("[Hero] Processing uploaded file: ContentType={ContentType}, Length={Length} bytes.", contentType, HeroBackgroundImage.Length);
                    if (!allowed.Contains(contentType))
                    {
                        _logger.LogWarning("[Hero] Rejected invalid image type: {ContentType}. Allowed: JPEG, PNG, GIF, WebP.", contentType);
                        return Json(new { success = false, message = "Invalid image type. Use JPEG, PNG, GIF, or WebP." });
                    }
                    using (var ms = new MemoryStream())
                    {
                        await HeroBackgroundImage.CopyToAsync(ms);
                        homePage.HeaderBackgroundImageData = ms.ToArray();
                    }
                    homePage.HeaderBackgroundImageContentType = contentType;
                    homePage.HeaderBackgroundImageUrl = null; // frontend will use /api/portfolio/hero-image
                    _logger.LogInformation("[Hero] Image stored in memory. Data length: {Bytes} bytes. Saving to DB.", homePage.HeaderBackgroundImageData?.Length ?? 0);
                }
                else
                {
                    // No file uploaded: use URL if provided, or clear stored image
                    homePage.HeaderBackgroundImageUrl = model.HeaderBackgroundImageUrl?.Trim() ?? "";
                    if (string.IsNullOrEmpty(homePage.HeaderBackgroundImageUrl))
                    {
                        homePage.HeaderBackgroundImageData = null;
                        homePage.HeaderBackgroundImageContentType = null;
                        _logger.LogInformation("[Hero] No file and no URL: cleared stored image and URL.");
                    }
                    else
                    {
                        homePage.HeaderBackgroundImageData = null;
                        homePage.HeaderBackgroundImageContentType = null;
                        _logger.LogInformation("[Hero] No file; using URL only. HeaderBackgroundImageUrl set (length {Len}).", homePage.HeaderBackgroundImageUrl?.Length ?? 0);
                    }
                }

                // Single save operation
                _logger.LogInformation("[Hero] Calling SaveChangesAsync...");
                await _context.SaveChangesAsync();
                _logger.LogInformation("[Hero] SaveChangesAsync completed. HeaderBackgroundImageData present: {HasData}, length: {Bytes}.",
                    homePage.HeaderBackgroundImageData != null, homePage.HeaderBackgroundImageData?.Length ?? 0);

                // Force cache refresh to ensure fresh data (only if it's the service with caching)
                if (_homePageService is Services.HomePageService homePageService)
                {
                    homePageService.ForceCacheRefresh();
                }
                var updatedHomePage = await _homePageService.GetHomePageAsync();

                // Broadcast WebSocket message to all connected clients
                await _webSocketService.BroadcastHeroDataUpdatedAsync();

                var scheme = Request.Scheme;
                if (scheme == "http" && Request.Host.Host?.Contains("onrender.com", StringComparison.OrdinalIgnoreCase) == true)
                    scheme = "https";
                var baseUrl = $"{scheme}://{Request.Host}";
                var hp = updatedHomePage ?? homePage;
                var hasStoredImage = hp.HeaderBackgroundImageData != null;
                var version = (hp.UpdatedAt ?? hp.CreatedAt).Ticks;
                var backgroundImageUrl = hasStoredImage
                    ? $"{baseUrl}/api/portfolio/hero-image?v={version}"
                    : (updatedHomePage?.HeaderBackgroundImageUrl ?? homePage.HeaderBackgroundImageUrl ?? "");

                return Json(new { 
                    success = true, 
                    message = "Hero section saved successfully!",
                    data = new
                    {
                        title = updatedHomePage?.HeaderTitle ?? homePage.HeaderTitle,
                        subtitle = updatedHomePage?.HeaderSubtitle ?? homePage.HeaderSubtitle,
                        description = updatedHomePage?.HeaderDescription ?? homePage.HeaderDescription,
                        backgroundImageUrl,
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
                var msg = ex.Message; var inner = ex.InnerException?.Message ?? "";
                _logger.LogError(ex, "[Hero] Error saving hero section. Message: {Msg}. Inner: {Inner}. HeroBackgroundImage: {HasFile}, Length: {Length}", msg, inner, HeroBackgroundImage != null, HeroBackgroundImage?.Length ?? 0);
                var isColumnError = msg.Contains("column", StringComparison.OrdinalIgnoreCase) || msg.Contains("does not exist", StringComparison.OrdinalIgnoreCase) || inner.Contains("column", StringComparison.OrdinalIgnoreCase);
                var userMessage = isColumnError
                    ? "Database may be missing hero image columns. On Render Postgres, run add-hero-image-columns.sql once (see project root), then try again."
                    : "An error occurred while saving the hero section. Check server logs for details.";
                return Json(new { success = false, message = userMessage });
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

        // ----- Features Section -----
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
                var dto = new FeaturesSectionEditDto
                {
                    SectionTitle = model.SectionTitle,
                    SectionSubtitle = model.SectionSubtitle,
                    Feature1Title = model.Feature1Title,
                    Feature1Subtitle = model.Feature1Subtitle,
                    Feature1Description = model.Feature1Description,
                    Feature1Link = model.Feature1Link,
                    Feature1LinkText = model.Feature1LinkText,
                    Feature2Title = model.Feature2Title,
                    Feature2Subtitle = model.Feature2Subtitle,
                    Feature2Description = model.Feature2Description,
                    Feature2Link = model.Feature2Link,
                    Feature2LinkText = model.Feature2LinkText,
                    Feature3Title = model.Feature3Title,
                    Feature3Subtitle = model.Feature3Subtitle,
                    Feature3Description = model.Feature3Description,
                    Feature3Link = model.Feature3Link,
                    Feature3LinkText = model.Feature3LinkText,
                    IsActive = model.IsActive,
                    DisplayOrder = model.DisplayOrder
                };
                var (success, message) = await _featuresSectionService.SaveFromDtoAsync(dto);
                if (success)
                    await _webSocketService.BroadcastMessageAsync(System.Text.Json.JsonSerializer.Serialize(new { type = "featuresDataUpdated" }));
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving features section");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFeaturesSection()
        {
            try
            {
                var data = await _featuresSectionService.GetAdminDtoAsync();
                if (data == null)
                    return Json(new { success = false, message = "Features section not found.", data = (object?)null });
                return Json(new { success = true, message = "Features section retrieved successfully.", data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving features section");
                return Json(new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetFeaturesTemplate")]
        public async Task<IActionResult> GetFeaturesTemplate(int id)
        {
            try
            {
                var template = await _context.FeaturesTemplates.FindAsync(id);
                if (template == null)
                    return Json(new { success = false, message = "Template not found.", data = (object?)null });
                var data = new
                {
                    id = template.Id,
                    nickname = template.Nickname ?? "",
                    sectionTitle = template.SectionTitle ?? "",
                    sectionSubtitle = template.SectionSubtitle ?? "",
                    sectionDescription = template.SectionDescription ?? "",
                    feature1Title = template.Feature1Title ?? "",
                    feature1Subtitle = template.Feature1Subtitle ?? "",
                    feature1Description = template.Feature1Description ?? "",
                    feature1Link = template.Feature1Link ?? "",
                    feature1LinkText = template.Feature1LinkText ?? "",
                    feature2Title = template.Feature2Title ?? "",
                    feature2Subtitle = template.Feature2Subtitle ?? "",
                    feature2Description = template.Feature2Description ?? "",
                    feature2Link = template.Feature2Link ?? "",
                    feature2LinkText = template.Feature2LinkText ?? "",
                    feature3Title = template.Feature3Title ?? "",
                    feature3Subtitle = template.Feature3Subtitle ?? "",
                    feature3Description = template.Feature3Description ?? "",
                    feature3Link = template.Feature3Link ?? "",
                    feature3LinkText = template.Feature3LinkText ?? "",
                    createdAt = template.CreatedAt,
                    updatedAt = template.UpdatedAt
                };
                return Json(new { success = true, message = "Features template retrieved successfully.", data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving features template");
                return Json(new { success = false, message = ex.Message, data = (object?)null });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetFeaturesTemplates")]
        public async Task<IActionResult> GetFeaturesTemplates()
        {
            try
            {
                var templates = await _context.FeaturesTemplates
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => new { id = t.Id, nickname = t.Nickname, sectionTitle = t.SectionTitle, sectionSubtitle = t.SectionSubtitle, createdAt = t.CreatedAt, updatedAt = t.UpdatedAt })
                    .ToListAsync();
                return Json(new { success = true, message = "Features templates retrieved successfully.", data = templates });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving features templates");
                return Json(new { success = true, data = new List<object>() });
            }
        }

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
                if (string.IsNullOrWhiteSpace(model.SectionTitle)) return Json(new { success = false, message = "Section Title is required." });
                if (string.IsNullOrWhiteSpace(model.Feature1Title)) return Json(new { success = false, message = "Feature 1 Title is required." });
                if (string.IsNullOrWhiteSpace(model.Feature1Subtitle)) return Json(new { success = false, message = "Feature 1 Subtitle is required." });
                if (string.IsNullOrWhiteSpace(model.Feature1Link)) return Json(new { success = false, message = "Feature 1 Link is required." });
                if (string.IsNullOrWhiteSpace(model.Feature2Title)) return Json(new { success = false, message = "Feature 2 Title is required." });
                if (string.IsNullOrWhiteSpace(model.Feature2Subtitle)) return Json(new { success = false, message = "Feature 2 Subtitle is required." });
                if (string.IsNullOrWhiteSpace(model.Feature2Link)) return Json(new { success = false, message = "Feature 2 Link is required." });
                if (string.IsNullOrWhiteSpace(model.Feature3Title)) return Json(new { success = false, message = "Feature 3 Title is required." });
                if (string.IsNullOrWhiteSpace(model.Feature3Subtitle)) return Json(new { success = false, message = "Feature 3 Subtitle is required." });
                if (string.IsNullOrWhiteSpace(model.Feature3Link)) return Json(new { success = false, message = "Feature 3 Link is required." });

                FeaturesTemplate template;
                if (model.Id == null || model.Id == 0)
                {
                    template = new FeaturesTemplate
                    {
                        Nickname = model.Nickname ?? "New Template",
                        SectionTitle = model.SectionTitle ?? "",
                        SectionSubtitle = model.SectionSubtitle ?? "",
                        Feature1Title = model.Feature1Title ?? "",
                        Feature1Subtitle = model.Feature1Subtitle ?? "",
                        Feature1Description = model.Feature1Description ?? "",
                        Feature1Link = model.Feature1Link ?? "",
                        Feature1LinkText = model.Feature1LinkText ?? "Learn more",
                        Feature2Title = model.Feature2Title ?? "",
                        Feature2Subtitle = model.Feature2Subtitle ?? "",
                        Feature2Description = model.Feature2Description ?? "",
                        Feature2Link = model.Feature2Link ?? "",
                        Feature2LinkText = model.Feature2LinkText ?? "Learn more",
                        Feature3Title = model.Feature3Title ?? "",
                        Feature3Subtitle = model.Feature3Subtitle ?? "",
                        Feature3Description = model.Feature3Description ?? "",
                        Feature3Link = model.Feature3Link ?? "",
                        Feature3LinkText = model.Feature3LinkText ?? "Learn more",
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.FeaturesTemplates.Add(template);
                }
                else
                {
                    template = await _context.FeaturesTemplates.FindAsync(model.Id);
                    if (template == null) return Json(new { success = false, message = "Template not found" });
                    template.Nickname = model.Nickname ?? template.Nickname;
                    template.SectionTitle = model.SectionTitle ?? template.SectionTitle;
                    template.SectionSubtitle = model.SectionSubtitle ?? "";
                    template.Feature1Title = model.Feature1Title ?? template.Feature1Title;
                    template.Feature1Subtitle = model.Feature1Subtitle ?? template.Feature1Subtitle;
                    template.Feature1Description = model.Feature1Description ?? "";
                    template.Feature1Link = model.Feature1Link ?? template.Feature1Link;
                    template.Feature1LinkText = model.Feature1LinkText ?? template.Feature1LinkText ?? "Learn more";
                    template.Feature2Title = model.Feature2Title ?? template.Feature2Title;
                    template.Feature2Subtitle = model.Feature2Subtitle ?? template.Feature2Subtitle;
                    template.Feature2Description = model.Feature2Description ?? "";
                    template.Feature2Link = model.Feature2Link ?? template.Feature2Link;
                    template.Feature2LinkText = model.Feature2LinkText ?? template.Feature2LinkText ?? "Learn more";
                    template.Feature3Title = model.Feature3Title ?? template.Feature3Title;
                    template.Feature3Subtitle = model.Feature3Subtitle ?? template.Feature3Subtitle;
                    template.Feature3Description = model.Feature3Description ?? "";
                    template.Feature3Link = model.Feature3Link ?? template.Feature3Link;
                    template.Feature3LinkText = model.Feature3LinkText ?? template.Feature3LinkText ?? "Learn more";
                    template.UpdatedAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = $"Features template '{template.Nickname}' saved successfully!", data = new { id = template.Id, nickname = template.Nickname } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving features template");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeaturesTemplate(int id)
        {
            try
            {
                var template = await _context.FeaturesTemplates.FindAsync(id);
                if (template == null) return Json(new { success = false, message = "Template not found" });
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

        // ----- CTA Section -----
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCTASection([FromForm] CTASectionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }
                var dto = new CTASectionEditDto { Title = model.Title, Subtitle = model.Subtitle, ButtonText = model.ButtonText, ButtonLink = model.ButtonLink };
                var (success, message) = await _ctaSectionService.SaveFromDtoAsync(dto);
                if (success)
                    await _webSocketService.BroadcastMessageAsync("{\"type\":\"ctaDataUpdated\",\"timestamp\":\"" + DateTime.UtcNow.ToString("O") + "\"}");
                return Json(new { success, message });
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
                var data = await _ctaSectionService.GetAdminDtoAsync();
                if (data == null)
                    return Json(new { success = false, message = "No CTA section found." });
                return Json(new { success = true, data });
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
                    return Json(new { success = false, message = "CTA template not found." });
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
                        lastModified = template.UpdatedAt ?? template.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting CTA template");
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
                var templates = await _context.CTATemplates
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => new { id = t.Id, nickname = t.Nickname ?? "", lastModified = t.UpdatedAt ?? t.CreatedAt })
                    .ToListAsync();
                return Json(new { success = true, message = "CTA templates retrieved successfully.", data = templates });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving CTA templates");
                return Json(new { success = true, data = new List<object>() });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCTATemplate([FromForm] CTATemplateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }
                if (string.IsNullOrWhiteSpace(model.Title)) return Json(new { success = false, message = "Title is required." });
                if (string.IsNullOrWhiteSpace(model.Subtitle)) return Json(new { success = false, message = "Subtitle is required." });
                if (string.IsNullOrWhiteSpace(model.ButtonText)) return Json(new { success = false, message = "Button text is required." });
                if (string.IsNullOrWhiteSpace(model.ButtonLink)) return Json(new { success = false, message = "Button link is required." });
                var existing = await _context.CTATemplates.FirstOrDefaultAsync(t => t.Nickname == model.Nickname);
                if (existing != null)
                    return Json(new { success = false, message = "A template with this nickname already exists. Please choose a different name." });

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
        public async Task<IActionResult> UpdateCTATemplate([FromForm] CTATemplateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }
                if (string.IsNullOrWhiteSpace(model.Title)) return Json(new { success = false, message = "Title is required." });
                if (string.IsNullOrWhiteSpace(model.Subtitle)) return Json(new { success = false, message = "Subtitle is required." });
                if (string.IsNullOrWhiteSpace(model.ButtonText)) return Json(new { success = false, message = "Button text is required." });
                if (string.IsNullOrWhiteSpace(model.ButtonLink)) return Json(new { success = false, message = "Button link is required." });
                var existingTemplate = await _context.CTATemplates.FindAsync(model.Id);
                if (existingTemplate == null)
                    return Json(new { success = false, message = "Template not found." });
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
                if (template == null) return Json(new { success = false, message = "Template not found" });
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNavSettings([FromForm] bool showGraphicDesignLink, [FromForm] bool showDesignLink)
        {
            try
            {
                var (success, message) = await _siteSettingsService.SaveNavSettingsAsync(showGraphicDesignLink, showDesignLink);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving nav settings");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSkillsLinkText([FromForm] string? linkText)
        {
            try
            {
                var (success, message) = await _siteSettingsService.SaveSkillsLinkTextAsync(linkText);
                if (success)
                    await _webSocketService.BroadcastMessageAsync(System.Text.Json.JsonSerializer.Serialize(new { type = "skillsDataUpdated" }));
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving skills link text");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSkillsCategory([FromForm] int? id, [FromForm] string title, [FromForm] string? description, [FromForm] string? skillsText, [FromForm] int displayOrder, [FromForm] bool isActive)
        {
            try
            {
                var (success, message, categoryId) = await _skillsCategoryService.SaveCategoryAsync(id, title, description, skillsText, displayOrder, isActive);
                if (success)
                    await _webSocketService.BroadcastMessageAsync(System.Text.Json.JsonSerializer.Serialize(new { type = "skillsDataUpdated" }));
                return Json(new { success, message, id = categoryId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving skills category");
                return Json(new { success = false, message = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkillsCategory([FromForm] int id)
        {
            try
            {
                var (success, message) = await _skillsCategoryService.DeleteCategoryAsync(id);
                if (success)
                    await _webSocketService.BroadcastMessageAsync(System.Text.Json.JsonSerializer.Serialize(new { type = "skillsDataUpdated" }));
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skills category");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadDefaultSkills([FromForm] string? __RequestVerificationToken)
        {
            try
            {
                var (success, message) = await _skillsCategoryService.LoadDefaultCategoriesIfMissingAsync();
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading default skills");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ----- Projects (with Vimeo video support) -----
        [HttpGet]
        [Authorize(Roles = "Admin,ReadOnly")]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                var list = await _projectService.GetAllAsync();
                var data = list.Select(p => new
                {
                    id = p.Id,
                    title = p.Title,
                    subtitle = p.Subtitle,
                    description = p.Description,
                    imageUrl = p.ImageUrl,
                    videoUrl = p.VideoUrl,
                    features = p.Features,
                    technologies = p.Technologies,
                    projectUrl = p.ProjectUrl,
                    githubUrl = p.GithubUrl,
                    completionDate = p.CompletionDate
                }).ToList();
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting projects");
                return Json(new { success = false, message = ex.Message, data = new List<object>() });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,ReadOnly")]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var p = await _projectService.GetByIdAsync(id);
                if (p == null) return Json(new { success = false, message = "Project not found." });
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = p.Id,
                        title = p.Title,
                        subtitle = p.Subtitle,
                        description = p.Description,
                        imageUrl = p.ImageUrl,
                        videoUrl = p.VideoUrl,
                        features = p.Features,
                        technologies = p.Technologies,
                        projectUrl = p.ProjectUrl,
                        githubUrl = p.GithubUrl,
                        completionDate = p.CompletionDate
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProject([FromForm] int? id, [FromForm] string title, [FromForm] string? subtitle, [FromForm] string? description, [FromForm] string? imageUrl, [FromForm] string? videoUrl, [FromForm] string? featuresJson, [FromForm] string? technologiesJson, [FromForm] string? projectUrl, [FromForm] string? githubUrl, [FromForm] DateTime? completionDate)
        {
            try
            {
                var features = ParseJsonStringList(featuresJson);
                var technologies = ParseJsonStringList(technologiesJson);
                var dto = new ProjectEditDto
                {
                    Id = id,
                    Title = title ?? "",
                    Subtitle = subtitle,
                    Description = description ?? "",
                    ImageUrl = imageUrl ?? "",
                    VideoUrl = videoUrl,
                    Features = features,
                    Technologies = technologies,
                    ProjectUrl = projectUrl ?? "",
                    GithubUrl = githubUrl ?? "",
                    CompletionDate = completionDate ?? DateTime.UtcNow
                };
                var (success, message, projectId) = await _projectService.SaveAsync(dto);
                if (success)
                    await _webSocketService.BroadcastMessageAsync(System.Text.Json.JsonSerializer.Serialize(new { type = "projectsDataUpdated" }));
                return Json(new { success, message, id = projectId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving project");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject([FromForm] int id)
        {
            try
            {
                var (success, message) = await _projectService.DeleteAsync(id);
                if (success)
                    await _webSocketService.BroadcastMessageAsync(System.Text.Json.JsonSerializer.Serialize(new { type = "projectsDataUpdated" }));
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project");
                return Json(new { success = false, message = ex.Message });
            }
        }

        private static List<string> ParseJsonStringList(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<string>();
            try
            {
                var list = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
                return list ?? new List<string>();
            }
            catch { return new List<string>(); }
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
            public string? Feature1Link { get; set; }
            public string? Feature1LinkText { get; set; }
            public string? Feature2Title { get; set; }
            public string? Feature2Subtitle { get; set; }
            public string? Feature2Description { get; set; }
            public string? Feature2Link { get; set; }
            public string? Feature2LinkText { get; set; }
            public string? Feature3Title { get; set; }
            public string? Feature3Subtitle { get; set; }
            public string? Feature3Description { get; set; }
            public string? Feature3Link { get; set; }
            public string? Feature3LinkText { get; set; }
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
            public string? Feature1Link { get; set; }
            public string? Feature1LinkText { get; set; }
            public string? Feature2Title { get; set; }
            public string? Feature2Subtitle { get; set; }
            public string? Feature2Description { get; set; }
            public string? Feature2Link { get; set; }
            public string? Feature2LinkText { get; set; }
            public string? Feature3Title { get; set; }
            public string? Feature3Subtitle { get; set; }
            public string? Feature3Description { get; set; }
            public string? Feature3Link { get; set; }
            public string? Feature3LinkText { get; set; }
        }

        public class CTASectionModel
        {
            public string? Title { get; set; }
            public string? Subtitle { get; set; }
            public string? ButtonText { get; set; }
            public string? ButtonLink { get; set; }
        }

        public class CTATemplateModel
        {
            public int Id { get; set; }
            public string? Nickname { get; set; }
            public string? Title { get; set; }
            public string? Subtitle { get; set; }
            public string? ButtonText { get; set; }
            public string? ButtonLink { get; set; }
        }
    }
}