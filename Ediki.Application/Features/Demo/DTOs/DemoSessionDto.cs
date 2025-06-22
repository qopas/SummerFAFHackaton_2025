using Ediki.Domain.Entities;
using Ediki.Domain.Enums;

namespace Ediki.Application.Features.Demo.DTOs;

public class DemoSessionDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HostUserId { get; set; } = string.Empty;
    public string HostName { get; set; } = string.Empty;
    public DemoSessionType Type { get; set; }
    public DemoSessionStatus Status { get; set; }
    public DateTime ScheduledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int MaxParticipants { get; set; }
    public int ParticipantCount { get; set; }
    public bool IsRecording { get; set; }
    public string? RecordingUrl { get; set; }
    public string? StreamKey { get; set; }
    public string? RoomId { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsPublic { get; set; }
    public bool RequireModeration { get; set; }
    public DemoSettings Settings { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class DemoParticipantDto
{
    public string Id { get; set; } = string.Empty;
    public string DemoSessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public DemoParticipantRole Role { get; set; }
    public DemoParticipantStatus Status { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public bool HasVideo { get; set; }
    public bool HasAudio { get; set; }
    public bool CanInteract { get; set; }
} 