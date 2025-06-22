using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ediki.Application.Interfaces;
using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Interfaces;
using Ediki.Infrastructure.Data;
using Ediki.Infrastructure.Repositories;
using Ediki.Infrastructure.Services;
using Task = System.Threading.Tasks.Task;

namespace Ediki.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database - Build connection string with Railway support
        var connectionString = BuildConnectionString() 
                            ?? configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured");
        }

        Console.WriteLine($"Using connection string with host: {ExtractHost(connectionString)}");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.CommandTimeout(configuration.GetValue<int>("Database:CommandTimeout", 30));
                npgsqlOptions.EnableRetryOnFailure(configuration.GetValue<int>("Database:MaxRetryCount", 3));
            }));

        // Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // JWT Authentication with improved environment variable support
        var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
                  ?? configuration["JwtSettings:SecretKey"] 
                  ?? configuration["JWT:Secret"];

        var jwtIssuer = Environment.GetEnvironmentVariable("API_BASE_URL")
                     ?? configuration["JwtSettings:Issuer"] 
                     ?? configuration["JWT:ValidIssuer"];

        var jwtAudience = Environment.GetEnvironmentVariable("API_BASE_URL")
                       ?? configuration["JwtSettings:Audience"] 
                       ?? configuration["JWT:ValidAudience"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT SecretKey not configured");
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                
                ValidAudience = jwtAudience ?? "https://localhost",
                ValidIssuer = jwtIssuer ?? "https://localhost",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
        services.AddScoped<ISprintRepository, SprintRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddHttpContextAccessor();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await context.Database.MigrateAsync();
        
        await SeedDataAsync(scope.ServiceProvider);
    }

    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // Create default roles
        var roles = new[] { "Admin", "User", "ProjectManager" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole 
                { 
                    Name = roleName,
                    Description = $"Default {roleName} role",
                    CreatedAt = DateTime.UtcNow
                };
                await roleManager.CreateAsync(role);
            }
        }

        // Create default admin user
        var adminEmail = "admin@ediki.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    private static string? BuildConnectionString()
    {
        var host = Environment.GetEnvironmentVariable("PGHOST");
        var port = Environment.GetEnvironmentVariable("PGPORT");
        var database = Environment.GetEnvironmentVariable("PGDATABASE");
        var username = Environment.GetEnvironmentVariable("PGUSER");
        var password = Environment.GetEnvironmentVariable("PGPASSWORD");

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database) || 
            string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        return $"Host={host};Port={port ?? "5432"};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
    }

    private static string ExtractHost(string connectionString)
    {
        if (connectionString.Contains("Host="))
        {
            var hostPart = connectionString.Split(';').FirstOrDefault(s => s.StartsWith("Host="));
            return hostPart?.Replace("Host=", "") ?? "unknown";
        }
        return "parsed-from-url";
    }
}
