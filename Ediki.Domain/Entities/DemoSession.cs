using Ediki.Domain.Enums;

namespace Ediki.Domain.Entities;

public class DemoSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HostUserId { get; set; } = string.Empty;
    public DemoSessionType Type { get; set; }
    public DemoSessionStatus Status { get; set; } = DemoSessionStatus.Scheduled;
    public DateTime ScheduledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int MaxParticipants { get; set; } = 50;
    public bool IsRecording { get; set; } = false;
    public string? RecordingUrl { get; set; }
    public string? StreamKey { get; set; }
    public string? RoomId { get; set; }
    public DemoSettings Settings { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Additional fields
    public int? DurationMinutes { get; set; } // Planned duration in minutes
    public bool IsPublic { get; set; } = false;
    public bool RequireModeration { get; set; } = false;

    // Navigation properties
    public ApplicationUser Host { get; set; } = null!;
    public List<DemoParticipant> Participants { get; set; } = new();
    public List<DemoRecording> Recordings { get; set; } = new();
    public List<DemoMessage> Messages { get; set; } = new();
    public List<DemoAction> Actions { get; set; } = new();
}

public class DemoParticipant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DemoSessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public DemoParticipantRole Role { get; set; }
    public DemoParticipantStatus Status { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }
    public bool HasVideo { get; set; } = false;
    public bool HasAudio { get; set; } = false;
    public bool CanInteract { get; set; } = false;

    // Navigation properties
    public DemoSession DemoSession { get; set; } = null!;
    public ApplicationUser? User { get; set; }
}

public class DemoRecording
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DemoSessionId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? StreamUrl { get; set; }
    public long FileSizeBytes { get; set; }
    public TimeSpan Duration { get; set; }
    public RecordingQuality Quality { get; set; }
    public RecordingStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();

    // Navigation properties
    public DemoSession DemoSession { get; set; } = null!;
}

public class DemoMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DemoSessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DemoMessageType Type { get; set; } = DemoMessageType.Chat;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsFromHost { get; set; } = false;

    // Navigation properties
    public DemoSession DemoSession { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}

public class DemoAction
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DemoSessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DemoActionType ActionType { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string ActionData { get; set; } = string.Empty; // JSON
    public TimeSpan TimestampInSession { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public DemoSession DemoSession { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}

public class DemoSettings
{
    public bool AllowChat { get; set; } = true;
    public bool AllowQuestions { get; set; } = true;
    public bool AllowScreenAnnotation { get; set; } = false;
    public bool AllowScreenShare { get; set; } = true;
    public bool AutoRecord { get; set; } = false;
    public bool RequireApproval { get; set; } = false;
    public RecordingQuality RecordingQuality { get; set; } = RecordingQuality.HD;
    public List<string> AllowedDomains { get; set; } = new();
} 