using Ediki.Domain.Common;
using Ediki.Application.Features.Demo.DTOs;
using Ediki.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Application.Features.Demo.Queries.GetDemoSessions;

public class GetDemoSessionsQueryHandler : IRequestHandler<GetDemoSessionsQuery, Result<List<DemoSessionDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetDemoSessionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<DemoSessionDto>>> Handle(GetDemoSessionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.DemoSessions
            .Include(ds => ds.Host)
            .Include(ds => ds.Participants)
            .AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(ds => ds.Status == request.Status.Value);
        }

        if (!string.IsNullOrEmpty(request.UserId))
        {
            query = query.Where(ds => ds.HostUserId == request.UserId || 
                                     ds.Participants.Any(p => p.UserId == request.UserId));
        }

        if (request.IsPublic.HasValue)
        {
            query = query.Where(ds => ds.IsPublic == request.IsPublic.Value);
        }

        var demoSessions = await query
            .OrderByDescending(ds => ds.ScheduledAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = demoSessions.Select(ds => new DemoSessionDto
        {
            Id = ds.Id,
            Title = ds.Title,
            Description = ds.Description,
            HostUserId = ds.HostUserId,
            HostName = $"{ds.Host.FirstName} {ds.Host.LastName}".Trim(),
            Type = ds.Type,
            Status = ds.Status,
            ScheduledAt = ds.ScheduledAt,
            StartedAt = ds.StartedAt,
            EndedAt = ds.EndedAt,
            MaxParticipants = ds.MaxParticipants,
            ParticipantCount = ds.Participants.Count(p => p.Status == Domain.Enums.DemoParticipantStatus.Joined),
            IsRecording = ds.IsRecording,
            RecordingUrl = ds.RecordingUrl,
            StreamKey = ds.StreamKey,
            RoomId = ds.RoomId,
            DurationMinutes = ds.DurationMinutes,
            IsPublic = ds.IsPublic,
            RequireModeration = ds.RequireModeration,
            Settings = ds.Settings,
            CreatedAt = ds.CreatedAt,
            UpdatedAt = ds.UpdatedAt
        }).ToList();

        return Result<List<DemoSessionDto>>.Success(dtos);
    }
} 