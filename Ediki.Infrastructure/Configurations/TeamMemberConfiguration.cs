using Ediki.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ediki.Infrastructure.Configurations;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");
        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.TeamId)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(tm => tm.UserId)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(tm => tm.Role)
            .IsRequired();

        builder.Property(tm => tm.JoinedAt);

        builder.Property(tm => tm.Progress)
            .IsRequired()
            .HasDefaultValue(0.0f);

        builder.Property(tm => tm.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(tm => tm.InvitedBy)
            .HasMaxLength(36);

        builder.Property(tm => tm.InvitedAt);

        builder.Property(tm => tm.AcceptedAt);

        // Foreign key relationship with Team
        builder.HasOne<Team>()
            .WithMany()
            .HasForeignKey(tm => tm.TeamId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key relationship with User
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(tm => tm.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key relationship with User (Invited By)
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(tm => tm.InvitedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Unique constraint on TeamId + UserId
        builder.HasIndex(tm => new { tm.TeamId, tm.UserId })
            .IsUnique();

        // Index on TeamId for better query performance
        builder.HasIndex(tm => tm.TeamId);

        // Index on UserId for better query performance
        builder.HasIndex(tm => tm.UserId);
    }
}