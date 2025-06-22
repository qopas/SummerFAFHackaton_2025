using Ediki.Domain.Enums;

namespace Ediki.Application.Features.Notifications.DTOs;

public class NotificationDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationPriority Priority { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public bool IsRead { get; set; }
    public List<string> MentionedUserIds { get; set; } = new();
    public string? CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
} 