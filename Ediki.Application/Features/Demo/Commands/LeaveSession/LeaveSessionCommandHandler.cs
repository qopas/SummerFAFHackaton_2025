using Ediki.Domain.Common;
using Ediki.Application.Interfaces;
using Ediki.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Application.Features.Demo.Commands.LeaveSession;

public class LeaveSessionCommandHandler : IRequestHandler<LeaveSessionCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public LeaveSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        var participant = await _context.DemoParticipants
            .FirstOrDefaultAsync(p => p.Id == request.ParticipantId && p.DemoSessionId == request.SessionId, 
                cancellationToken);

        if (participant == null)
        {
            return Result<bool>.Failure("Participant not found in this session");
        }

        if (participant.Status != DemoParticipantStatus.Joined)
        {
            return Result<bool>.Failure("Participant is not currently in the session");
        }

        // Update participant status
        participant.Status = DemoParticipantStatus.Left;
        participant.LeftAt = DateTime.UtcNow;
        participant.HasVideo = false;
        participant.HasAudio = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
} 