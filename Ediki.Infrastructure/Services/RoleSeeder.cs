using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;

namespace Ediki.Infrastructure.Services;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("RoleSeeder"); 

        var roles = new[]
        {
            new ApplicationRole { Name = RoleNames.Admin, Description = "Administrator with full access" },
            new ApplicationRole { Name = RoleNames.User, Description = "Regular user with limited access" },
            new ApplicationRole { Name = RoleNames.Creator, Description = "Content creator with special permissions" }
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    logger.LogInformation("Role {RoleName} created successfully", role.Name);
                }
                else
                {
                    logger.LogError("Failed to create role {RoleName}: {Errors}", 
                        role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}