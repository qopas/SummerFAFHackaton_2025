using Ediki.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = "admin-role-id",
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator with full access",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "8f434346-85c4-4c5b-b7e8-4c5e8b7f9d1c"
            },
            new ApplicationRole
            {
                Id = "user-role-id",
                Name = "User",
                NormalizedName = "USER",
                Description = "Regular user with limited access",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "7d532240-74c3-3b4a-a6f7-3b4e7c8f0e2d"
            },
            new ApplicationRole
            {
                Id = "creator-role-id",
                Name = "Creator",
                NormalizedName = "CREATOR",
                Description = "Content creator with special permissions",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ConcurrencyStamp = "6c421130-63b2-2a39-95e6-2a3d6b7e9c1f"
            }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}