using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Ediki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Infrastructure.Repositories;

public class NotificationRepository(ApplicationDbContext context) : INotificationRepository
{
    public async Task<Notification?> GetByIdAsync(string id)
    {
        return await context.Notifications
            .Include(n => n.User)
            .Include(n => n.CreatedByUser)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool? isRead = null, NotificationType? type = null, int skip = 0, int take = 50)
    {
        var query = context.Notifications
            .Include(n => n.User)
            .Include(n => n.CreatedByUser)
            .Where(n => n.UserId == userId);

        if (isRead.HasValue)
        {
            query = query.Where(n => n.IsRead == isRead.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(n => n.Type == type.Value);
        }

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        context.Notifications.Add(notification);
        await context.SaveChangesAsync();
        return notification;
    }

    public async Task<Notification> UpdateAsync(Notification notification)
    {
        context.Notifications.Update(notification);
        await context.SaveChangesAsync();
        return notification;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var notification = await context.Notifications.FindAsync(id);
        if (notification == null)
            return false;

        context.Notifications.Remove(notification);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAsReadAsync(string id)
    {
        var notification = await context.Notifications.FindAsync(id);
        if (notification == null)
            return false;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAllAsReadAsync(string userId)
    {
        var notifications = await context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        if (!notifications.Any())
            return true;

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Notification>> GetPriorityNotificationsAsync(string userId, NotificationPriority priority)
    {
        return await context.Notifications
            .Include(n => n.User)
            .Include(n => n.CreatedByUser)
            .Where(n => n.UserId == userId && n.Priority == priority && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
} 