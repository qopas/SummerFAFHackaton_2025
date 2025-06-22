using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ediki.Domain.Entities;
using System.Text.Json;

namespace Ediki.Infrastructure.Data.Configurations;

public class DemoSessionConfiguration : IEntityTypeConfiguration<DemoSession>
{
    public void Configure(EntityTypeBuilder<DemoSession> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasMaxLength(50);
            
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Description)
            .HasMaxLength(1000);
            
        builder.Property(x => x.HostUserId)
            .IsRequired()
            .HasMaxLength(450);
            
        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(x => x.StreamKey)
            .HasMaxLength(100);
            
        builder.Property(x => x.RoomId)
            .HasMaxLength(100);
            
        builder.Property(x => x.RecordingUrl)
            .HasMaxLength(500);
            
        builder.Property(x => x.DurationMinutes);
        
        builder.Property(x => x.IsPublic)
            .HasDefaultValue(false);
            
        builder.Property(x => x.RequireModeration)
            .HasDefaultValue(false);

        // Configure DemoSettings as owned type
        builder.OwnsOne(x => x.Settings, settings =>
        {
            settings.Property(s => s.AllowedDomains)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
                .HasColumnType("text");
        });
        
        // Relationships
        builder.HasOne(x => x.Host)
            .WithMany()
            .HasForeignKey(x => x.HostUserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(x => x.Participants)
            .WithOne(x => x.DemoSession)
            .HasForeignKey(x => x.DemoSessionId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(x => x.Recordings)
            .WithOne(x => x.DemoSession)
            .HasForeignKey(x => x.DemoSessionId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(x => x.Messages)
            .WithOne(x => x.DemoSession)
            .HasForeignKey(x => x.DemoSessionId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(x => x.Actions)
            .WithOne(x => x.DemoSession)
            .HasForeignKey(x => x.DemoSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(x => x.HostUserId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ScheduledAt);
        builder.HasIndex(x => x.CreatedAt);
    }
}

public class DemoParticipantConfiguration : IEntityTypeConfiguration<DemoParticipant>
{
    public void Configure(EntityTypeBuilder<DemoParticipant> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasMaxLength(50);
            
        builder.Property(x => x.DemoSessionId)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);
            
        builder.Property(x => x.Email)
            .HasMaxLength(256);
            
        builder.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();
        
        // Relationships
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Indexes
        builder.HasIndex(x => new { x.DemoSessionId, x.UserId })
            .IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.JoinedAt);
    }
}

public class DemoRecordingConfiguration : IEntityTypeConfiguration<DemoRecording>
{
    public void Configure(EntityTypeBuilder<DemoRecording> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasMaxLength(50);
            
        builder.Property(x => x.DemoSessionId)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(x => x.FilePath)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(x => x.StreamUrl)
            .HasMaxLength(500);
            
        builder.Property(x => x.Quality)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Metadata)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, object>())
            .HasColumnType("text");
        
        // Indexes
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.StartedAt);
    }
}

public class DemoMessageConfiguration : IEntityTypeConfiguration<DemoMessage>
{
    public void Configure(EntityTypeBuilder<DemoMessage> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasMaxLength(50);
            
        builder.Property(x => x.DemoSessionId)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);
            
        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(2000);
            
        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();
        
        // Relationships
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes
        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.Type);
    }
}

public class DemoActionConfiguration : IEntityTypeConfiguration<DemoAction>
{
    public void Configure(EntityTypeBuilder<DemoAction> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasMaxLength(50);
            
        builder.Property(x => x.DemoSessionId)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);
            
        builder.Property(x => x.ActionType)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(x => x.EntityType)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.EntityId)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.ActionData)
            .HasMaxLength(4000);
        
        // Relationships
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes
        builder.HasIndex(x => x.ActionType);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.TimestampInSession);
    }
} 