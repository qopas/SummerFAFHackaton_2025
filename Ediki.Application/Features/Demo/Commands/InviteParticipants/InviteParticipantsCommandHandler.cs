using Ediki.Domain.Common;
using Ediki.Application.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ediki.Application.Features.Notifications.Commands.CreateNotification;

namespace Ediki.Application.Features.Demo.Commands.InviteParticipants;

public class InviteParticipantsCommandHandler : IRequestHandler<InviteParticipantsCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public InviteParticipantsCommandHandler(
        IApplicationDbContext context,
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Result<bool>> Handle(InviteParticipantsCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.DemoSessions
            .Include(s => s.Host)
            .FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);

        if (session == null)
        {
            return Result<bool>.Failure("Demo session not found");
        }

        var existingParticipants = await _context.DemoParticipants
            .Where(p => p.DemoSessionId == request.SessionId)
            .Select(p => p.Email)
            .ToListAsync(cancellationToken);

        var newEmails = request.ParticipantEmails
            .Where(email => !existingParticipants.Contains(email))
            .ToList();

        if (!newEmails.Any())
        {
            return Result<bool>.Success(true);
        }

        var participants = new List<DemoParticipant>();
        foreach (var email in newEmails)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            var participant = new DemoParticipant
            {
                Id = Guid.NewGuid().ToString(),
                DemoSessionId = request.SessionId,
                UserId = user?.Id,
                Email = email,
                DisplayName = user.NormalizedUserName ?? email.Split('@')[0],
                Role = DemoParticipantRole.Participant,
                Status = DemoParticipantStatus.Invited,
                JoinedAt = DateTime.Now,
                LeftAt = null,
                HasVideo = false,
                HasAudio = false,
                CanInteract = true
            };

            participants.Add(participant);
        }

        _context.DemoParticipants.AddRange(participants);
        await _context.SaveChangesAsync(cancellationToken);
        
        if (request.SendNotification)
        {
            foreach (var participant in participants)
            {
                if (!string.IsNullOrEmpty(participant.UserId))
                {
                    var notificationCommand = new CreateNotificationCommand
                    {
                        UserId = participant.UserId,
                        Type = NotificationType.DemoInvitation,
                        Priority = NotificationPriority.Normal,
                        Title = $"Demo Session Invitation: {session.Title}",
                        Message = !string.IsNullOrEmpty(request.InviteMessage) 
                            ? $"{session.Host.UserName ?? session.Host.Email} invited you to join '{session.Title}'. Message: {request.InviteMessage}"
                            : $"{session.Host.UserName ?? session.Host.Email} invited you to join the demo session '{session.Title}' scheduled for {session.ScheduledAt:MMM dd, yyyy 'at' hh:mm tt}.",
                        ActionUrl = $"/demo/sessions/{session.Id}",
                        RelatedEntityId = session.Id,
                        RelatedEntityType = "DemoSession"
                    };

                    await _mediator.Send(notificationCommand, cancellationToken);
                }
            }
        }

        return Result<bool>.Success(true);
    }
} 