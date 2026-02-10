using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Services;
using Portfolio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register application services
builder.Services.AddScoped<IHomePageService, HomePageService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<WebSocketService>();

// Add CORS for development and production
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPortfolio", policy =>
    {
        policy.WithOrigins(
                "https://codespinner.net",
                "http://codespinner.net",
                "https://localhost:44406", 
                "http://localhost:44406",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.IsEssential = true;
        options.Cookie.Name = "Portfolio.Admin.Auth";
        options.Cookie.MaxAge = null; // This makes it a session cookie
    });

// Prefer Render Postgres DATABASE_URL when set; otherwise use DefaultConnection (e.g. local SQL Server)
var rawConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = rawConnectionString?.Trim();
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Set DATABASE_URL (Render) or ConnectionStrings:DefaultConnection (appsettings.json).");
}

// Convert postgres:// URL (e.g. Render) to Npgsql key=value format to avoid "format does not conform" errors
if (connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
    connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
{
    try
    {
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo?.Split(new[] { ':' }, 2, StringSplitOptions.None) ?? Array.Empty<string>();
        var user = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : "";
        var pass = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var db = string.IsNullOrEmpty(uri.AbsolutePath) ? "" : uri.AbsolutePath.TrimStart('/');
        var port = uri.Port > 0 ? uri.Port : 5432;
        var sslMode = "Require"; // Render and most cloud Postgres expect SSL
        connectionString = $"Host={uri.Host};Port={port};Database={db};Username={user};Password={pass};SslMode={sslMode};";
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("DATABASE_URL must be a valid postgres:// URL.", ex);
    }
}

// When DATABASE_URL is set (e.g. Render), treat as Postgres so we never pass a Postgres URL to SQL Server
var fromEnv = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL"));
var isPostgres = fromEnv || connectionString.StartsWith("Host=", StringComparison.OrdinalIgnoreCase) || connectionString.Contains("Database=");
builder.Services.AddDbContext<PortfolioContext>(options =>
{
    if (isPostgres)
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
            npgsqlOptions.CommandTimeout(60);
        });
    }
    else
    {
        options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60);
        });
    }
});

var app = builder.Build();

// We do NOT run db.Database.Migrate() here: migrations are SQL Server-specific and fail on Postgres.
// To add hero image columns on Render: run add-hero-image-columns.sql once in your Postgres DB.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
// Configure static files with debugging
var staticFileOptions = new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
};



app.UseStaticFiles(staticFileOptions);

// Add CORS middleware for both development and production
app.UseCors("AllowPortfolio");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Add WebSocket middleware
app.UseWebSockets();

// WebSocket endpoint for real-time updates
app.Map("/ws/portfolio", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var webSocketService = context.RequestServices.GetRequiredService<WebSocketService>();
        var connectionId = Guid.NewGuid().ToString();
        
        webSocketService.AddConnection(connectionId, webSocket);
        
        try
        {
            await HandleWebSocketConnection(webSocket, webSocketService, connectionId);
        }
        finally
        {
            webSocketService.RemoveConnection(connectionId);
        }
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.UseEndpoints(endpoints =>
{


    // Redirect root to admin login
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/Admin/Login");
        return Task.CompletedTask;
    });

    // Map admin routes first with higher priority
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{action=Login}/{id?}",
        defaults: new { controller = "Admin" });

    // Map API routes
    endpoints.MapControllerRoute(
        name: "api",
        pattern: "api/{controller=Portfolio}/{action=Index}/{id?}");

    // Map default routes
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Ensure the wwwroot directory exists
if (!Directory.Exists(Path.Combine(app.Environment.ContentRootPath, "wwwroot")))
{
    Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "wwwroot"));
}

// Graceful shutdown handling
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("Application is shutting down...");
    
    // Close all WebSocket connections
    var webSocketService = app.Services.GetRequiredService<WebSocketService>();
    webSocketService.CloseAllConnections();
    
    Console.WriteLine("All WebSocket connections closed.");
});

app.Run();

// WebSocket connection handler
async Task HandleWebSocketConnection(WebSocket webSocket, WebSocketService webSocketService, string connectionId)
{
    var buffer = new byte[1024 * 4];
    var cancellationTokenSource = new CancellationTokenSource();
    
    try
    {
        while (webSocket.State == WebSocketState.Open && !cancellationTokenSource.Token.IsCancellationRequested)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);
            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                
                // Handle incoming messages if needed
                if (message == "ping")
                {
                    var response = Encoding.UTF8.GetBytes("pong");
                    await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, cancellationTokenSource.Token);
                }
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationTokenSource.Token);
                break;
            }
        }
    }
    catch (OperationCanceledException)
    {
        // Normal cancellation during shutdown
        Console.WriteLine($"WebSocket connection {connectionId} cancelled during shutdown");
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"WebSocket error for connection {connectionId}: {ex.Message}");
    }
    finally
    {
        cancellationTokenSource.Cancel();
        if (webSocket.State == WebSocketState.Open)
        {
            try
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }
            catch
            {
                // Ignore errors during shutdown
            }
        }
    }
}
