﻿using Microsoft.AspNetCore.Identity;

namespace Ediki.Domain.Entities;

public class ApplicationUser : IdentityUser<string>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    public ApplicationUser()
    {
        Id = Guid.NewGuid().ToString();
    }
}
