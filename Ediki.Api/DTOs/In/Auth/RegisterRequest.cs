using Ediki.Application.Commands.Auth.Register;
using Ediki.Api.DTOs.In;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace Ediki.Api.DTOs.In.Auth;

public class RegisterRequest : IRequestIn<RegisterCommand>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    
    // New fields
    public ProjectRole PreferredRole { get; set; }
    public int Xp { get; set; } = 0;
    public int Level { get; set; } = 1;
    public List<string>? Badges { get; set; }
    public int CompletedProjects { get; set; } = 0;
    public List<string>? Skills { get; set; }
    public string? University { get; set; }
    public int? GraduationYear { get; set; }
    public string? Location { get; set; }
    public SocialLinks? SocialLinks { get; set; }

    public RegisterCommand Convert()
    {
        return new RegisterCommand(
            Email, 
            Password, 
            ConfirmPassword, 
            FirstName, 
            LastName, 
            Role,
            PreferredRole,
            Xp,
            Level,
            Badges,
            CompletedProjects,
            Skills,
            University,
            GraduationYear,
            Location,
            SocialLinks);
    }
}
