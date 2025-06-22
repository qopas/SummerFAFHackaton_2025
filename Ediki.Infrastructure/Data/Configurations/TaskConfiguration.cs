using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Ediki.Infrastructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasMaxLength(36)
            .IsRequired();
            
        builder.Property(t => t.SprintId)
            .HasMaxLength(36);
            
        builder.Property(t => t.ProjectId)
            .HasMaxLength(36)
            .IsRequired();
            
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(t => t.Description)
            .HasMaxLength(2000);
            
        builder.Property(t => t.AssigneeId)
            .HasMaxLength(36);
            
        builder.Property(t => t.Status)
            .IsRequired();
            
        builder.Property(t => t.Priority)
            .IsRequired();
            
        builder.Property(t => t.EstimatedHours);
        builder.Property(t => t.ActualHours);
        
        builder.Property(t => t.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("TEXT");
            
        builder.Property(t => t.Dependencies)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("TEXT");
            
        builder.Property(t => t.DueDate);
        builder.Property(t => t.CompletedAt);
        
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(36)
            .IsRequired();
            
        builder.Property(t => t.CreatedAt)
            .IsRequired();
            
        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(t => t.Sprint)
            .WithMany(s => s.Tasks)
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.SetNull);
            
        builder.HasOne(t => t.Project)
            .WithMany()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(t => t.Assignee)
            .WithMany()
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);
            
        builder.HasOne(t => t.CreatedByUser)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(t => t.SprintId);
        builder.HasIndex(t => t.ProjectId);
        builder.HasIndex(t => t.AssigneeId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.CreatedBy);
        builder.HasIndex(t => t.DueDate);
    }
} 