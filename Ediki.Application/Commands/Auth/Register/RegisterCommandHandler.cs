using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Application.Commands.Auth.Register;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;

namespace Ediki.Application.Commands.Auth.Register;

public class RegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result<RegisterResult>.Failure("User with this email already exists");
        }

        if (request.Password != request.ConfirmPassword)
        {
            return Result<RegisterResult>.Failure("Passwords do not match");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            
            // New fields
            PreferredRole = request.PreferredRole,
            Xp = request.Xp,
            Level = request.Level,
            Badges = request.Badges ?? new List<string>(),
            CompletedProjects = request.CompletedProjects,
            Skills = request.Skills,
            University = request.University,
            GraduationYear = request.GraduationYear,
            Location = request.Location,
            SocialLinks = request.SocialLinks
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result<RegisterResult>.Failure(errors);
        }

        var roleName = request.Role;
        if (!RoleNames.GetAllRoles().Contains(roleName))
        {
            roleName = RoleNames.User;
        }

        if (await roleManager.RoleExistsAsync(roleName))
        {
            await userManager.AddToRoleAsync(user, roleName);
        }

        var registerResult = new RegisterResult
        {
            UserId = user.Id,
            Email = user.Email,
            Message = "User registered successfully"
        };

        return Result<RegisterResult>.Success(registerResult);
    }
}
