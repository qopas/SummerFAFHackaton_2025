using Ediki.Domain.Common;
using Ediki.Application.Features.Demo.DTOs;
using Ediki.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Application.Features.Demo.Queries.GetParticipants;

public class GetParticipantsQueryHandler : IRequestHandler<GetParticipantsQuery, Result<List<DemoParticipantDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetParticipantsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<DemoParticipantDto>>> Handle(GetParticipantsQuery request, CancellationToken cancellationToken)
    {
        var sessionExists = await _context.DemoSessions
            .AnyAsync(ds => ds.Id == request.SessionId, cancellationToken);

        if (!sessionExists)
        {
            return Result<List<DemoParticipantDto>>.Failure("Demo session not found");
        }

        var participants = await _context.DemoParticipants
            .Include(p => p.User)
            .Where(p => p.DemoSessionId == request.SessionId)
            .OrderBy(p => p.JoinedAt)
            .ToListAsync(cancellationToken);

        var dtos = participants.Select(p => new DemoParticipantDto
        {
            Id = p.Id,
            DemoSessionId = p.DemoSessionId,
            UserId = p.UserId,
            Email = p.Email,
            DisplayName = p.DisplayName,
            Role = p.Role,
            Status = p.Status,
            JoinedAt = p.JoinedAt,
            LeftAt = p.LeftAt,
            HasVideo = p.HasVideo,
            HasAudio = p.HasAudio,
            CanInteract = p.CanInteract
        }).ToList();

        return Result<List<DemoParticipantDto>>.Success(dtos);
    }
} 