using Ediki.Domain.Common;
using Ediki.Application.Features.Demo.DTOs;
using Ediki.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Application.Features.Demo.Queries.GetDemoSession;

public class GetDemoSessionQueryHandler : IRequestHandler<GetDemoSessionQuery, Result<DemoSessionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDemoSessionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DemoSessionDto>> Handle(GetDemoSessionQuery request, CancellationToken cancellationToken)
    {
        var demoSession = await _context.DemoSessions
            .Include(ds => ds.Host)
            .Include(ds => ds.Participants)
            .Include(ds => ds.Recordings)
            .FirstOrDefaultAsync(ds => ds.Id == request.Id, cancellationToken);

        if (demoSession == null)
        {
            return Result<DemoSessionDto>.Failure("Demo session not found");
        }

        var dto = new DemoSessionDto
        {
            Id = demoSession.Id,
            Title = demoSession.Title,
            Description = demoSession.Description,
            HostUserId = demoSession.HostUserId,
            HostName = $"{demoSession.Host.FirstName} {demoSession.Host.LastName}".Trim(),
            Type = demoSession.Type,
            Status = demoSession.Status,
            ScheduledAt = demoSession.ScheduledAt,
            StartedAt = demoSession.StartedAt,
            EndedAt = demoSession.EndedAt,
            MaxParticipants = demoSession.MaxParticipants,
            ParticipantCount = demoSession.Participants.Count(p => p.Status == Domain.Enums.DemoParticipantStatus.Joined),
            IsRecording = demoSession.IsRecording,
            RecordingUrl = demoSession.RecordingUrl,
            StreamKey = demoSession.StreamKey,
            RoomId = demoSession.RoomId,
            DurationMinutes = demoSession.DurationMinutes,
            IsPublic = demoSession.IsPublic,
            RequireModeration = demoSession.RequireModeration,
            Settings = demoSession.Settings,
            CreatedAt = demoSession.CreatedAt,
            UpdatedAt = demoSession.UpdatedAt
        };

        return Result<DemoSessionDto>.Success(dto);
    }
} 