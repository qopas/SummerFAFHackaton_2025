using Microsoft.AspNetCore.Identity;
using Ediki.Domain.Enums;

namespace Ediki.Domain.Entities;

public class ApplicationUser : IdentityUser<string>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public PreferredRole PreferredRole { get; set; } = PreferredRole.NotSet;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    public ApplicationUser()
    {
        Id = Guid.NewGuid().ToString();
    }
}
