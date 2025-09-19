using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Services;
using Portfolio.Services.Interfaces;
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
                "https://localhost:3000", 
                "http://localhost:3000",
                "https://localhost:44406", 
                "http://localhost:44406",
                "https://codespinner.netlify.app",
                "https://professionalportfolio-9a6n.onrender.com",
                "http://professionalportfolio-9a6n.onrender.com",
                "https://yourportfolio.com"
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

// Configure PostgreSQL connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration");

builder.Services.AddDbContext<PortfolioContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
        npgsqlOptions.CommandTimeout(60);
    });
});

var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PortfolioContext>();
    try
    {
        Console.WriteLine("Testing database connection...");
        Console.WriteLine($"Connection string starts with: {connectionString?.Substring(0, Math.Min(30, connectionString?.Length ?? 0))}...");
        
        // Test database connection first
        if (await context.Database.CanConnectAsync())
        {
            Console.WriteLine("Database connection successful. Applying migrations...");
            await context.Database.MigrateAsync();
            Console.WriteLine("Database migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("Database connection failed during startup, but app will continue.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying database migrations: {ex.Message}");
        Console.WriteLine($"Full exception: {ex}");
        Console.WriteLine("App will continue without database migrations.");
    }
}

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
app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
});

app.UseEndpoints(endpoints =>
{
    // WebSocket endpoint for real-time updates
    endpoints.Map("/client/hubs/portfolio", async context =>
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            try
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                var webSocketService = context.RequestServices.GetRequiredService<WebSocketService>();
                var connectionId = Guid.NewGuid().ToString();
                
                Console.WriteLine($"WebSocket connection established: {connectionId}");
                webSocketService.AddConnection(connectionId, webSocket);
                
                try
                {
                    await HandleWebSocketConnection(webSocket, webSocketService, connectionId);
                }
                finally
                {
                    webSocketService.RemoveConnection(connectionId);
                    Console.WriteLine($"WebSocket connection closed: {connectionId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket connection error: {ex.Message}");
                context.Response.StatusCode = 500;
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    });

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
lifetime.ApplicationStopping.Register(async () =>
{
    Console.WriteLine("Application is shutting down...");
    
    // Close all WebSocket connections
    var webSocketService = app.Services.GetRequiredService<WebSocketService>();
    await webSocketService.CloseAllConnections();
    
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
