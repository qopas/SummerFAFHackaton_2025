using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ediki.Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired();

        builder.Property(p => p.ShortDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Tags)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(p => p.RolesNeeded)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => Enum.Parse<ProjectRole>(r))
                    .ToList());

        builder.Property(p => p.MaxParticipants)
            .IsRequired();

        builder.Property(p => p.CurrentParticipants)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.Difficulty)
            .IsRequired();

        builder.Property(p => p.Duration)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.DurationInWeeks)
            .IsRequired();

        builder.Property(p => p.Deadline)
            .IsRequired();

        builder.Property(p => p.Company)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.CompanyLogo)
            .HasMaxLength(2000);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasDefaultValue(ProjectStatus.Recruiting);

        builder.Property(p => p.Progress)
            .IsRequired()
            .HasDefaultValue(0.0f);

        builder.Property(p => p.IsPublic)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.IsFeatured)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.Deliverables)
            .HasConversion(
                v => v != null ? string.Join(',', v) : null,
                v => v != null ? v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null);

        builder.OwnsOne(p => p.Rewards, rewards =>
        {
            rewards.Property(r => r.Xp)
                .HasColumnName("RewardXp")
                .HasDefaultValue(100);
            rewards.Property(r => r.Certificates)
                .HasColumnName("RewardCertificates")
                .HasDefaultValue(false);
            rewards.Property(r => r.Recommendations)
                .HasColumnName("RewardRecommendations")
                .HasDefaultValue(false);
        });

        builder.OwnsMany(p => p.Resources, resource =>
        {
            resource.ToTable("ProjectResources");
            
            resource.WithOwner().HasForeignKey("ProjectId");
            
            resource.Property<int>("Id")
                .ValueGeneratedOnAdd();
            resource.HasKey("Id");
            
            resource.Property(r => r.Title)
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(200);
            resource.Property(r => r.Url)
                .HasColumnName("Url")
                .IsRequired()
                .HasMaxLength(2000);
            resource.Property(r => r.Type)
                .HasColumnName("Type")
                .IsRequired();
            
            resource.Property<DateTime>("CreatedAt")
                .HasColumnName("CreatedAt")
                .ValueGeneratedOnAdd();
        });

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.CreatedById)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
} 