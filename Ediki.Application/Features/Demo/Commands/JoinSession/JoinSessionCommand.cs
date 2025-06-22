using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Demo.Commands.JoinSession;

public class JoinSessionCommand : IRequest<Result<string>>
{
    public string SessionId { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public bool HasVideo { get; set; } = false;
    public bool HasAudio { get; set; } = false;
} 