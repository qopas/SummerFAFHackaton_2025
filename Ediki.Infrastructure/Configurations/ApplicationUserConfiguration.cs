using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ediki.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Configure the SocialLinks as an owned type
        builder.OwnsOne(u => u.SocialLinks, socialLinks =>
        {
            socialLinks.Property(sl => sl.Github)
                .HasMaxLength(500)
                .HasColumnName("SocialLinks_Github");
            
            socialLinks.Property(sl => sl.Linkedin)
                .HasMaxLength(500)
                .HasColumnName("SocialLinks_Linkedin");
            
            socialLinks.Property(sl => sl.Portfolio)
                .HasMaxLength(500)
                .HasColumnName("SocialLinks_Portfolio");
        });

        // Configure the PreferredRole enum
        builder.Property(u => u.PreferredRole)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(PreferredRole.NotSet);

        // Configure Skills as delimited string with value comparer
        builder.Property(u => u.Skills)
            .HasConversion(
                v => v == null ? null : string.Join(';', v),
                v => v == null ? null : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasMaxLength(2000)
            .Metadata.SetValueComparer(new ValueComparer<List<string>?>(
                (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c == null ? null : c.ToList()));

        // Configure Badges as delimited string with value comparer
        builder.Property(u => u.Badges)
            .HasConversion(
                v => v == null || v.Count == 0 ? "" : string.Join(';', v),
                v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasMaxLength(5000)
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        // Configure other properties
        builder.Property(u => u.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.University)
            .HasMaxLength(100);

        builder.Property(u => u.Location)
            .HasMaxLength(100);

        builder.Property(u => u.Xp)
            .HasDefaultValue(0);

        builder.Property(u => u.Level)
            .HasDefaultValue(1);

        builder.Property(u => u.CompletedProjects)
            .HasDefaultValue(0);

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        // Use PostgreSQL compatible function for default timestamp
        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("NOW()");

        // Add index on email for performance
        builder.HasIndex(u => u.Email)
            .IsUnique();

        // Add index on PreferredRole for filtering
        builder.HasIndex(u => u.PreferredRole);
    }
} 