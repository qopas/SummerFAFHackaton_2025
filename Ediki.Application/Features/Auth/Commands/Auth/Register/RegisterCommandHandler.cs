using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Application.Commands.Auth.Register;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;

namespace Ediki.Application.Commands.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
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
            PreferredRole = request.PreferredRole,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
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

        if (await _roleManager.RoleExistsAsync(roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
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
