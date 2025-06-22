using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace Ediki.Application.DTOs.Auth;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    
    // New fields
    public ProjectRole PreferredRole { get; set; }
    public int Xp { get; set; } = 0;
    public int Level { get; set; } = 1;
    public List<string> Badges { get; set; } = new();
    public int CompletedProjects { get; set; } = 0;
    public List<string>? Skills { get; set; }
    public string? University { get; set; }
    public int? GraduationYear { get; set; }
    public string? Location { get; set; }
    public SocialLinks? SocialLinks { get; set; }
}
