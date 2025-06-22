using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ediki.Application.Commands.Auth.Login;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;

namespace Ediki.Application.Commands.Auth.Login;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IConfiguration configuration) : IRequestHandler<LoginCommand, Result<LoginResult>>
{
    public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<LoginResult>.Failure("Invalid email or password");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Result<LoginResult>.Failure("Invalid email or password");
        }

        var token = await GenerateJwtToken(user);
        var userRoles = await userManager.GetRolesAsync(user);

        var loginResult = new LoginResult
        {
            Token = token,
            RefreshToken = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = userRoles.ToList(),
                CreatedAt = user.CreatedAt,
                
                // New fields
                PreferredRole = user.PreferredRole,
                Xp = user.Xp,
                Level = user.Level,
                Badges = user.Badges,
                CompletedProjects = user.CompletedProjects,
                Skills = user.Skills,
                University = user.University,
                GraduationYear = user.GraduationYear,
                Location = user.Location,
                SocialLinks = user.SocialLinks
            }
        };

        return Result<LoginResult>.Success(loginResult);
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
            new("PreferredRole", user.PreferredRole.ToString()),
            new("Level", user.Level.ToString()),
            new("Xp", user.Xp.ToString())
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? "default-secret-key"));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"] ?? "https://localhost",
            audience: configuration["JWT:ValidAudience"] ?? "https://localhost",
            expires: DateTime.Now.AddHours(24),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
