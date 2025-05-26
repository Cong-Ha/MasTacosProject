using System.Text;
using MasTacos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// === Add Services ===

// Add CORS with environment-specific configuration
if (builder.Environment.IsDevelopment())
{
    // In development, allow any origin
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.SetIsOriginAllowed(_ => true)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });
}
else
{
    // In production, only allow GitHub Pages and handle Railway's domain
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.WithOrigins(
                    "https://cong-ha.github.io",
                    "https://cong-ha-mastacos--80.prod1a.defang.dev"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains(); // Allow Railway's dynamic subdomains
        });
    });
}

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DB context
builder.Services.AddDbContext<MasTacos.Data.MasTacosContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // For Railway deployment, construct connection string from environment variables if available
    if (Environment.GetEnvironmentVariable("MYSQLHOST") != null)
    {
        connectionString = $"Server={Environment.GetEnvironmentVariable("MYSQLHOST")};" +
                         $"Port={Environment.GetEnvironmentVariable("MYSQLPORT")};" +
                         $"Database={Environment.GetEnvironmentVariable("MYSQLDATABASE")};" +
                         $"User={Environment.GetEnvironmentVariable("MYSQLUSER")};" +
                         $"Password={Environment.GetEnvironmentVariable("MYSQLPASSWORD")};";
    }

    // Use a specific server version instead of auto-detect
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));
    
    options.UseMySql(
        connectionString,
        serverVersion,
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }
    );
},
ServiceLifetime.Scoped
);

// Add Identity
builder.Services.AddIdentity<MasTacos.Models.ApplicationUser, IdentityRole>(options => 
    {
        // Configure Identity options
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    
        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
    
        // User settings
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<MasTacos.Data.MasTacosContext>()
    .AddDefaultTokenProviders();

// Configure authentication
builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = "Bearer";
        options.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer(options =>
    {
        // Configure JWT Bearer options
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Register repositories
builder.Services.AddScoped<MasTacos.Repository.Interfaces.ICustomerRepository, MasTacos.Repository.CustomerRepository>();
builder.Services.AddScoped<MasTacos.Repository.Interfaces.IMenuItemRepository, MasTacos.Repository.MenuItemRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.IOrderRepository, MasTacos.Repository.OrderRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.IReservationRepository, MasTacos.Repository.ReservationRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.ISurveyRepository, MasTacos.Repository.SurveyRepository>();
// builder.Services.AddScoped<MasTacos.Repository.Interfaces.ITimeSlotRepository, MasTacos.Repository.TimeSlotRepository>();

var app = builder.Build();

// === Configure Middleware ===

// Enable CORS globally (before Authorization)
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    // Disable caching of Swagger UI in dev
    app.Use(async (context, next) =>
    {
        context.Response.Headers["Cache-Control"] = "no-store";
        await next();
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Only use HTTPS redirection in non-Docker, production environments
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Health check: API is running");

// Seed users and roles AFTER middleware is configured
// This ensures the database is available before attempting to seed
await Task.Run(async () =>
{
    // Wait a moment for the database to be ready
    await Task.Delay(5000);
    
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        // Ensure roles exist
        string[] roleNames = { "Admin", "Customer" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                logger.LogInformation("Created role {Role}", roleName);
            }
        }
        
        // Create an admin user if it doesn't exist
        var adminEmail = "admin@mastacos.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                logger.LogInformation("Admin user created successfully");
                await userManager.AddToRoleAsync(admin, "Admin");
                logger.LogInformation("Added Admin role to admin user");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    logger.LogError("Error creating admin user: {Error}", error.Description);
                }
            }
        }
        
        // Create a customer user if it doesn't exist
        var customerEmail = "customer@example.com";
        if (await userManager.FindByEmailAsync(customerEmail) == null)
        {
            var customer = new ApplicationUser
            {
                UserName = customerEmail,
                Email = customerEmail,
                FirstName = "Test",
                LastName = "Customer",
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(customer, "Customer123!");
            if (result.Succeeded)
            {
                logger.LogInformation("Customer user created successfully");
                await userManager.AddToRoleAsync(customer, "Customer");
                logger.LogInformation("Added Customer role to customer user");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    logger.LogError("Error creating customer user: {Error}", error.Description);
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding users and roles");
    }
});

app.Run();