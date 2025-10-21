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
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.IsEssential = true;
        options.Cookie.Name = "Portfolio.Admin.Auth";
        options.Cookie.MaxAge = null; // This makes it a session cookie
    });

// Configure PostgreSQL connection
Console.WriteLine("=== DEBUG CONNECTION STRING ===");
var configConnString = builder.Configuration.GetConnectionString("DefaultConnection");
var envConnString = Environment.GetEnvironmentVariable("DATABASE_URL");
var envConnString2 = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

Console.WriteLine($"Config connection string: '{configConnString}'");
Console.WriteLine($"DATABASE_URL env var: '{envConnString}'");
Console.WriteLine($"ConnectionStrings__DefaultConnection env var: '{envConnString2}'");

var rawConnectionString = configConnString ?? envConnString ?? envConnString2
    ?? throw new InvalidOperationException("No connection string found in any source");

// Convert Render PostgreSQL URL to Npgsql format if needed
string connectionString;
Console.WriteLine("=== CONNECTION STRING PARSING ===");
Console.WriteLine($"Raw connection string length: {rawConnectionString?.Length ?? 0}");
Console.WriteLine($"Raw connection string starts with: {rawConnectionString?.Substring(0, Math.Min(20, rawConnectionString?.Length ?? 0))}...");

if (rawConnectionString.StartsWith("postgresql://") || rawConnectionString.StartsWith("postgres://"))
{
    Console.WriteLine("🔄 Converting PostgreSQL URL format to Npgsql format...");
    try
    {
        // Parse the PostgreSQL URL format: postgresql://user:password@host:port/database
        var uri = new Uri(rawConnectionString);
        var host = uri.Host;
        var port = uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');
        var userInfo = uri.UserInfo.Split(':');
        var username = userInfo[0];
        var password = userInfo.Length > 1 ? userInfo[1] : "";
        
        Console.WriteLine($"Parsed components:");
        Console.WriteLine($"  Host: {host}");
        Console.WriteLine($"  Port: {port}");
        Console.WriteLine($"  Database: {database}");
        Console.WriteLine($"  Username: {username}");
        Console.WriteLine($"  Password length: {password?.Length ?? 0} characters");
        
        // Build Npgsql connection string
        connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;Include Error Detail=true";
        Console.WriteLine($"✅ Converted to Npgsql format successfully");
        Console.WriteLine($"Final connection string: {connectionString.Replace(password, "***")}");
    }
    catch (Exception parseEx)
    {
        Console.WriteLine($"❌ Error parsing PostgreSQL URL: {parseEx.Message}");
        Console.WriteLine("Falling back to raw connection string...");
        connectionString = rawConnectionString;
    }
}
else
{
    Console.WriteLine("ℹ️ Using raw connection string format (already in Npgsql format)");
    connectionString = rawConnectionString;
}

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
    
    // Suppress pending model changes warning to allow migration
    options.ConfigureWarnings(warnings => 
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PortfolioContext>();
    try
    {
        Console.WriteLine("=== STARTING DATABASE CONNECTION DIAGNOSTICS ===");
        Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
        Console.WriteLine($"Connection string format: {(rawConnectionString.StartsWith("postgresql://") ? "PostgreSQL URL" : "Npgsql format")}");
        Console.WriteLine($"Final connection string: {connectionString}");
        
        // Test database connection with detailed logging
        bool connected = false;
        for (int i = 0; i < 3; i++)
        {
            try
            {
                Console.WriteLine($"=== DATABASE CONNECTION ATTEMPT {i + 1}/3 ===");
                Console.WriteLine($"Attempting to connect to: {connectionString.Split(';')[0]}"); // Show only Host part
                
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                bool canConnect = await context.Database.CanConnectAsync();
                stopwatch.Stop();
                
                Console.WriteLine($"CanConnectAsync() returned: {canConnect} (took {stopwatch.ElapsedMilliseconds}ms)");
                
                if (canConnect)
                {
                    Console.WriteLine("✅ Database connection successful!");
                    
                    // Check if database exists
                    Console.WriteLine("Checking if database exists...");
                    var dbExists = await context.Database.EnsureCreatedAsync();
                    Console.WriteLine($"Database creation result: {(dbExists ? "Created new database" : "Database already exists")}");
                    
                    // Check pending migrations
                    Console.WriteLine("Checking for pending migrations...");
                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    Console.WriteLine($"Pending migrations count: {pendingMigrations.Count()}");
                    
                    if (pendingMigrations.Any())
                    {
                        Console.WriteLine("Pending migrations:");
                        foreach (var migration in pendingMigrations)
                        {
                            Console.WriteLine($"  - {migration}");
                        }
                        
                        Console.WriteLine("Applying migrations...");
                        await context.Database.MigrateAsync();
                        Console.WriteLine("✅ Database migrations applied successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No pending migrations found.");
                    }
                    
                    // Test basic query
                    Console.WriteLine("Testing basic database query...");
                    var adminCount = await context.Admins.CountAsync();
                    Console.WriteLine($"Admin table query successful. Found {adminCount} admin records.");
                    
                    // Auto-create admin user from environment variables if none exist
                    if (adminCount == 0)
                    {
                        await CreateInitialAdminUser(context);
                    }
                    
                    connected = true;
                    break;
                }
                else
                {
                    Console.WriteLine("❌ CanConnectAsync() returned false");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Connection attempt {i + 1} failed:");
                Console.WriteLine($"Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"Exception Message: {ex.Message}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                    Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message}");
                }
                
                // Log specific Npgsql errors
                if (ex.Message.Contains("Format of the initialization string"))
                {
                    Console.WriteLine("🔍 CONNECTION STRING FORMAT ERROR DETECTED");
                    Console.WriteLine($"Raw connection string: {rawConnectionString}");
                    Console.WriteLine($"Parsed connection string: {connectionString}");
                }
                
                if (ex.Message.Contains("timeout"))
                {
                    Console.WriteLine("🔍 TIMEOUT ERROR DETECTED - Network or firewall issue");
                }
                
                if (ex.Message.Contains("authentication") || ex.Message.Contains("password"))
                {
                    Console.WriteLine("🔍 AUTHENTICATION ERROR DETECTED - Check credentials");
                }
                
                if (ex.Message.Contains("does not exist"))
                {
                    Console.WriteLine("🔍 DATABASE/HOST NOT FOUND ERROR DETECTED");
                }
                
                if (i < 2) 
                {
                    Console.WriteLine($"Waiting 5 seconds before retry {i + 2}/3...");
                    await Task.Delay(5000);
                }
            }
        }
        
        if (!connected)
        {
            Console.WriteLine("=== DATABASE CONNECTION FAILED ===");
            Console.WriteLine("❌ All database connection attempts failed, but app will continue.");
            Console.WriteLine("The application will run without database functionality.");
        }
        else
        {
            Console.WriteLine("=== DATABASE CONNECTION SUCCESSFUL ===");
            Console.WriteLine("✅ Database is ready and migrations are applied.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("=== CRITICAL DATABASE ERROR ===");
        Console.WriteLine($"❌ Critical error in database initialization: {ex.Message}");
        Console.WriteLine($"Exception Type: {ex.GetType().Name}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
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

    // Serve API info at root
    endpoints.MapGet("/", context =>
    {
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync("{\"message\": \"Portfolio API is running\", \"version\": \"1.0.0\"}");
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

// Function to create initial admin users from environment variables
static async Task CreateInitialAdminUser(PortfolioContext context)
{
    try
    {
        Console.WriteLine("=== CREATING INITIAL ADMIN USERS ===");
        
        // Check for primary admin
        var adminUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        
        // Check for second admin
        var admin2Username = Environment.GetEnvironmentVariable("ADMIN2_USERNAME");
        var admin2Password = Environment.GetEnvironmentVariable("ADMIN2_PASSWORD");
        
        var adminsToCreate = new List<(string username, string password)>();
        
        if (!string.IsNullOrEmpty(adminUsername) && !string.IsNullOrEmpty(adminPassword))
        {
            adminsToCreate.Add((adminUsername, adminPassword));
        }
        
        if (!string.IsNullOrEmpty(admin2Username) && !string.IsNullOrEmpty(admin2Password))
        {
            adminsToCreate.Add((admin2Username, admin2Password));
        }
        
        if (adminsToCreate.Count == 0)
        {
            Console.WriteLine("⚠️ No admin environment variables set.");
            Console.WriteLine("Set these environment variables to auto-create admin users:");
            Console.WriteLine("  - ADMIN_USERNAME=your_email@domain.com");
            Console.WriteLine("  - ADMIN_PASSWORD=your_secure_password");
            Console.WriteLine("  - ADMIN2_USERNAME=second_email@domain.com (optional)");
            Console.WriteLine("  - ADMIN2_PASSWORD=second_secure_password (optional)");
            return;
        }
        
        Console.WriteLine($"Found {adminsToCreate.Count} admin user(s) to create");
        
        foreach (var (username, password) in adminsToCreate)
        {
            // Check if admin already exists
            var existingAdmin = await context.Admins
                .Where(a => a.Username == username || a.Email == username)
                .FirstOrDefaultAsync();
                
            if (existingAdmin != null)
            {
                Console.WriteLine($"ℹ️ Admin user {username} already exists, skipping...");
                continue;
            }
            
            Console.WriteLine($"Creating admin user: {username}");
            
            // Generate secure BCrypt hash
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, 12);
            
            // Determine role based on which admin this is
            var role = username == adminUsername ? "Admin" : "ReadOnly";
            
            var admin = new Portfolio.Models.Portfolio.Admin
            {
                Username = username,
                Email = username, // Using email as username
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Role = role
            };
            
            Console.WriteLine($"   Role: {role}");
            
            context.Admins.Add(admin);
            Console.WriteLine($"✅ Admin user {username} prepared for creation");
        }
        
        // Save all new admin users
        var changesSaved = await context.SaveChangesAsync();
        Console.WriteLine($"✅ {changesSaved} admin user(s) created successfully!");
        
        // Log final admin count
        var totalAdmins = await context.Admins.CountAsync();
        Console.WriteLine($"Total admin users in database: {totalAdmins}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error creating initial admin users: {ex.Message}");
        Console.WriteLine($"Exception Type: {ex.GetType().Name}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
    }
}

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
