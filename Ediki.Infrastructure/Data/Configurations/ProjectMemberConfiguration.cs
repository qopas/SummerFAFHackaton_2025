using Ediki.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ediki.Infrastructure.Data.Configurations;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("ProjectMembers");
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.ProjectId)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(pm => pm.UserId)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(pm => pm.Role)
            .IsRequired();

        builder.Property(pm => pm.JoinedAt)
            .IsRequired();

        builder.Property(pm => pm.Progress)
            .IsRequired()
            .HasDefaultValue(0.0f);

        builder.Property(pm => pm.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(pm => pm.IsProjectLead)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pm => pm.InvitedBy)
            .HasMaxLength(36);

        builder.Property(pm => pm.InvitedAt);

        builder.Property(pm => pm.AcceptedAt);

        // Relationships
        builder.HasOne(pm => pm.Project)
            .WithMany()
            .HasForeignKey(pm => pm.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pm => pm.User)
            .WithMany()
            .HasForeignKey(pm => pm.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pm => pm.InvitedByUser)
            .WithMany()
            .HasForeignKey(pm => pm.InvitedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(pm => new { pm.ProjectId, pm.UserId })
            .IsUnique();

        builder.HasIndex(pm => pm.ProjectId);
        builder.HasIndex(pm => pm.UserId);
        builder.HasIndex(pm => pm.Role);
        builder.HasIndex(pm => pm.IsActive);
        builder.HasIndex(pm => pm.IsProjectLead);
        builder.HasIndex(pm => pm.InvitedBy);
    }
} 