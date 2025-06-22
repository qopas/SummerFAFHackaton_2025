using Ediki.Application.Features.Notifications.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQuery : IRequest<Result<List<NotificationDto>>>
{
    public bool? IsRead { get; set; }
    public NotificationType? Type { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 50;
} 