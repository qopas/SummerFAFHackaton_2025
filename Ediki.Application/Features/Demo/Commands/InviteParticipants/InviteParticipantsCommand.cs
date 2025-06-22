using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Demo.Commands.InviteParticipants;

public class InviteParticipantsCommand : IRequest<Result<bool>>
{
    public string SessionId { get; set; } = string.Empty;
    public List<string> ParticipantEmails { get; set; } = new();
    public string? InviteMessage { get; set; }
    public bool SendNotification { get; set; } = true;
} 