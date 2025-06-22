using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommand : IRequest<Result<bool>>
{
    public string NotificationId { get; set; } = string.Empty;
} 