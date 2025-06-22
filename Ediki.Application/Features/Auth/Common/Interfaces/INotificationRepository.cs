using Ediki.Domain.Entities;
using Ediki.Domain.Enums;

namespace Ediki.Application.Common.Interfaces;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(string id);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool? isRead = null, NotificationType? type = null, int skip = 0, int take = 50);
    Task<int> GetUnreadCountAsync(string userId);
    Task<Notification> CreateAsync(Notification notification);
    Task<Notification> UpdateAsync(Notification notification);
    Task<bool> DeleteAsync(string id);
    Task<bool> MarkAsReadAsync(string id);
    Task<bool> MarkAllAsReadAsync(string userId);
    Task<IEnumerable<Notification>> GetPriorityNotificationsAsync(string userId, NotificationPriority priority);
} 