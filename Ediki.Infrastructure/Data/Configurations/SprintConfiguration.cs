using Ediki.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Ediki.Infrastructure.Data.Configurations;

public class SprintConfiguration : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .HasMaxLength(36)
            .IsRequired();
            
        builder.Property(s => s.ProjectId)
            .HasMaxLength(36)
            .IsRequired();
            
        builder.Property(s => s.Name)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(s => s.Description)
            .HasMaxLength(1000);
            
        builder.Property(s => s.StartDate)
            .IsRequired();
            
        builder.Property(s => s.EndDate)
            .IsRequired();
            
        builder.Property(s => s.Status)
            .IsRequired();
            
        builder.Property(s => s.Order)
            .IsRequired();
            
        builder.Property(s => s.Goals)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("TEXT");
            
        builder.Property(s => s.Deliverables)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("TEXT");
            
        builder.Property(s => s.CreatedAt)
            .IsRequired();
            
        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(s => s.Project)
            .WithMany()
            .HasForeignKey(s => s.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(s => s.Tasks)
            .WithOne(t => t.Sprint)
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.ProjectId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => new { s.ProjectId, s.Order });
    }
} 