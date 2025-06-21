using Microsoft.AspNetCore.Identity;

namespace Ediki.Domain.Entities;

public class ApplicationRole : IdentityRole<string>
{
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ApplicationRole()
    {
        Id = Guid.NewGuid().ToString();
    }
}
