using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
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

        public AdminController(PortfolioContext context, ILogger<AdminController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
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
                    return BadRequest(string.Join(", ", errors));
                }

                _logger.LogInformation($"login attempt for username: {model.Username}");

                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    _logger.LogWarning("login attempt with empty username or password");
                    return BadRequest("Username and password are required.");
                }

                var hashedPassword = HashPassword(model.Password);
                _logger.LogInformation($"Input password hash: {hashedPassword}");

                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Username == model.Username);

                if (admin == null)
                {
                    _logger.LogWarning($"No admin found with username: {model.Username}");
                    return BadRequest("Invalid username or password.");
                }

                _logger.LogInformation($"Stored password hash: {admin.PasswordHash}");
                _logger.LogInformation($"Password match: {admin.PasswordHash == hashedPassword}");

                if (admin.PasswordHash != hashedPassword)
                {
                    _logger.LogWarning($"Password mismatch for username: {model.Username}");
                    return BadRequest("Invalid username or password.");
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
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View();
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

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}