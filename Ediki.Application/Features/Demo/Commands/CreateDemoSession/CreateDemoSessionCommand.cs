using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Demo.Commands.CreateDemoSession;

public class CreateDemoSessionCommand : IRequest<Result<CreateDemoSessionResponse>>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DemoSessionType Type { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int MaxParticipants { get; set; } = 50;
    public bool AutoRecord { get; set; } = false;
    public RecordingQuality RecordingQuality { get; set; } = RecordingQuality.HD;
    public bool? AllowScreenShare { get; set; }
    public int? Duration { get; set; }
    public bool? IsPublic { get; set; }
    public bool? RequireModeration { get; set; }
}

public class CreateDemoSessionResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public string StreamKey { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public DemoSessionStatus Status { get; set; }
} 