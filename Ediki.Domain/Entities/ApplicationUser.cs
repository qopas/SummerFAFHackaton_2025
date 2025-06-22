using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Ediki.Domain.Entities;

public class ApplicationUser : IdentityUser<string>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    // New fields
    public PreferredRole PreferredRole { get; set; } = PreferredRole.NotSet;
    public int Xp { get; set; } = 0;
    public int Level { get; set; } = 1;
    public List<string> Badges { get; set; } = new();
    public int CompletedProjects { get; set; } = 0;
    public List<string>? Skills { get; set; }
    public string? University { get; set; }
    public int? GraduationYear { get; set; }
    public string? Location { get; set; }
    public SocialLinks? SocialLinks { get; set; }
    
    public ApplicationUser()
    {
        Id = Guid.NewGuid().ToString();
    }
}
