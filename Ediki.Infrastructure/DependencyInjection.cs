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
        // Database - Build connection string from environment variables with fallback to configuration
        var connectionString = BuildConnectionString(configuration);
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
            
        // Register IApplicationDbContext
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

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

        // JWT Authentication
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
                
                ValidAudience = configuration["JWT:ValidAudience"] ?? "https://localhost",
                ValidIssuer = configuration["JWT:ValidIssuer"] ?? "https://localhost",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? "default-secret-key"))
            };
        });

        // Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
        services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
        services.AddScoped<ISprintRepository, SprintRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddHttpContextAccessor();

        return services;
    }
    
    private static string BuildConnectionString(IConfiguration configuration)
    {
        var connectionString = BuildConnectionStringFromEnv() 
                              ?? configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured");
        }

        return connectionString;
    }

    private static string? BuildConnectionStringFromEnv()
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
    
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        
        try
        {
            // Apply pending migrations
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"Applying {pendingMigrations.Count()} pending migrations...");
                await context.Database.MigrateAsync();
                Console.WriteLine("Database migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("Database is up to date. No migrations to apply.");
            }
            
            await EnsureRolesAsync(roleManager);
            
            Console.WriteLine("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization failed: {ex.Message}");
            throw;
        }
    }

    private static async Task EnsureRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        var defaultRoles = new[]
        {
            new { Name = "Admin", Description = "Administrator with full access" },
            new { Name = "User", Description = "Regular user with limited access" },
            new { Name = "Creator", Description = "Content creator with special permissions" }
        };

        foreach (var roleInfo in defaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleInfo.Name))
            {
                var role = new ApplicationRole
                {
                    Name = roleInfo.Name,
                    Description = roleInfo.Description,
                    CreatedAt = DateTime.UtcNow
                };
                
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Created role: {roleInfo.Name}");
                }
                else
                {
                    Console.WriteLine($"Failed to create role {roleInfo.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
