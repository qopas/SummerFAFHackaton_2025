using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Demo.Commands.LeaveSession;

public class LeaveSessionCommand : IRequest<Result<bool>>
{
    public string SessionId { get; set; } = string.Empty;
    public string ParticipantId { get; set; } = string.Empty;
} 