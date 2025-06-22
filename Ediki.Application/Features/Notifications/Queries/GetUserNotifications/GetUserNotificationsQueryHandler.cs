using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Notifications.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQueryHandler(
    INotificationRepository notificationRepository,
    ICurrentUserService currentUserService) : IRequestHandler<GetUserNotificationsQuery, Result<List<NotificationDto>>>
{
    public async Task<Result<List<NotificationDto>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

            var notifications = await notificationRepository.GetUserNotificationsAsync(
                userId, 
                request.IsRead, 
                request.Type, 
                request.Skip, 
                request.Take);

            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                Priority = n.Priority,
                Title = n.Title,
                Message = n.Message,
                ActionUrl = n.ActionUrl,
                RelatedEntityId = n.RelatedEntityId,
                RelatedEntityType = n.RelatedEntityType,
                IsRead = n.IsRead,
                MentionedUserIds = n.MentionedUserIds,
                CreatedByUserId = n.CreatedByUserId,
                CreatedByUserName = n.CreatedByUser != null ? $"{n.CreatedByUser.FirstName} {n.CreatedByUser.LastName}" : string.Empty,
                CreatedAt = n.CreatedAt,
                ReadAt = n.ReadAt
            }).ToList();

            return Result<List<NotificationDto>>.Success(notificationDtos);
        }
        catch (Exception ex)
        {
            return Result<List<NotificationDto>>.Failure(ex.Message);
        }
    }
} 