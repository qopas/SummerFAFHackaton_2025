using Ediki.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ediki.Infrastructure.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.ProjectId)
            .IsRequired()
            .HasMaxLength(36);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.MaxMembers)
            .IsRequired()
            .HasDefaultValue(5);

        builder.Property(t => t.IsComplete)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.InviteCode)
            .HasMaxLength(10);

        builder.Property(t => t.TeamLead)
            .HasMaxLength(36);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt);

        // Foreign key relationship with Project
        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key relationship with User (Team Lead)
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(t => t.TeamLead)
            .OnDelete(DeleteBehavior.SetNull);

        // Unique constraint on InviteCode
        builder.HasIndex(t => t.InviteCode)
            .IsUnique()
            .HasFilter("\"InviteCode\" IS NOT NULL");

        // Index on ProjectId for better query performance
        builder.HasIndex(t => t.ProjectId);
    }
}