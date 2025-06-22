using Ediki.Domain.Enums;

namespace Ediki.Domain.Entities;

public class Notification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public string? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public bool IsRead { get; set; } = false;
    public List<string> MentionedUserIds { get; set; } = new();
    public string? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ApplicationUser? CreatedByUser { get; set; }
} 