using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Services;
using Portfolio.Services.Interfaces;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
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

// Add CORS for development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactDevServer", policy =>
        {
            policy.WithOrigins("https://localhost:44406", "http://localhost:44406")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });
}

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

// Configure MySQL connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");
}

var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));

builder.Services.AddDbContext<PortfolioContext>(options =>
{
    options.UseMySql(connectionString, serverVersion, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        mysqlOptions.CommandTimeout(60);
        mysqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add CORS middleware for development
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowReactDevServer");
}

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
    // Map admin routes first with higher priority
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{action=Login}/{id?}",
        defaults: new { controller = "Admin" });

    // Map default routes
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// SPA fallback - only for non-admin routes
app.MapWhen(context => !context.Request.Path.StartsWithSegments("/Admin"), builder =>
{
    builder.UseSpa(spa =>
    {
        if (app.Environment.IsDevelopment())
        {
            spa.UseProxyToSpaDevelopmentServer("https://localhost:44406");
        }
    });
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
