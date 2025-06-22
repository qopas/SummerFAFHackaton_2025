using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Ediki.Domain.Entities;

namespace Ediki.Infrastructure.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Id)
            .HasMaxLength(36)
            .IsRequired();
            
        builder.Property(n => n.UserId)
            .HasMaxLength(36)
            .IsRequired();
            
        builder.Property(n => n.Type)
            .IsRequired();
            
        builder.Property(n => n.Priority)
            .IsRequired();
            
        builder.Property(n => n.Title)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(n => n.Message)
            .HasMaxLength(2000)
            .IsRequired();
            
        builder.Property(n => n.ActionUrl)
            .HasMaxLength(500);
            
        builder.Property(n => n.RelatedEntityId)
            .HasMaxLength(36);
            
        builder.Property(n => n.RelatedEntityType)
            .HasMaxLength(100);
            
        builder.Property(n => n.IsRead)
            .IsRequired();
            
        builder.Property(n => n.MentionedUserIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("TEXT");
            
        builder.Property(n => n.CreatedByUserId)
            .HasMaxLength(36);
            
        builder.Property(n => n.CreatedAt)
            .IsRequired();
            
        builder.Property(n => n.ReadAt);

        builder.HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(n => n.CreatedByUser)
            .WithMany()
            .HasForeignKey(n => n.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => n.Type);
        builder.HasIndex(n => n.Priority);
        builder.HasIndex(n => n.IsRead);
        builder.HasIndex(n => n.CreatedAt);
        builder.HasIndex(n => new { n.UserId, n.IsRead });
    }
} 