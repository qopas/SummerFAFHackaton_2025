using Ediki.Domain.Common;
using Ediki.Application.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Application.Features.Demo.Commands.JoinSession;

public class JoinSessionCommandHandler : IRequestHandler<JoinSessionCommand, Result<string>>
{
    private readonly IApplicationDbContext _context;

    public JoinSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(JoinSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.DemoSessions
            .Include(s => s.Participants)
            .FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);

        if (session == null)
        {
            return Result<string>.Failure("Demo session not found");
        }

        if (session.Status != DemoSessionStatus.Live && session.Status != DemoSessionStatus.Scheduled)
        {
            return Result<string>.Failure("Demo session is not available for joining");
        }

        // Check if session is at capacity
        var currentParticipantCount = session.Participants.Count(p => p.Status == DemoParticipantStatus.Joined);
        if (currentParticipantCount >= session.MaxParticipants)
        {
            return Result<string>.Failure("Demo session is at full capacity");
        }

        // Find existing participant or create new one
        DemoParticipant? participant = null;

        if (!string.IsNullOrEmpty(request.UserId))
        {
            participant = session.Participants.FirstOrDefault(p => p.UserId == request.UserId);
        }
        else if (!string.IsNullOrEmpty(request.Email))
        {
            participant = session.Participants.FirstOrDefault(p => p.Email == request.Email);
        }

        if (participant == null)
        {
            // Create new participant
            participant = new DemoParticipant
            {
                Id = Guid.NewGuid().ToString(),
                DemoSessionId = request.SessionId,
                UserId = request.UserId,
                Email = request.Email ?? "anonymous@demo.com",
                DisplayName = request.DisplayName ?? "Anonymous User",
                Role = DemoParticipantRole.Participant,
                Status = DemoParticipantStatus.Joined,
                JoinedAt = DateTime.UtcNow,
                HasVideo = request.HasVideo,
                HasAudio = request.HasAudio,
                CanInteract = true
            };

            _context.DemoParticipants.Add(participant);
        }
        else
        {
            // Update existing participant
            participant.Status = DemoParticipantStatus.Joined;
            participant.JoinedAt = DateTime.UtcNow;
            participant.LeftAt = null;
            participant.HasVideo = request.HasVideo;
            participant.HasAudio = request.HasAudio;
        }

        // Update session status if needed
        if (session.Status == DemoSessionStatus.Scheduled)
        {
            session.Status = DemoSessionStatus.Live;
            session.StartedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(participant.Id);
    }
} 