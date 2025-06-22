using Ediki.Application.Features.Notifications.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommand : IRequest<Result<NotificationDto>>
{
    public string UserId { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public List<string> MentionedUserIds { get; set; } = new();
} 