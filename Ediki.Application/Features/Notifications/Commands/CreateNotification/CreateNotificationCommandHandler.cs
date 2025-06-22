using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Notifications.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using MediatR;

namespace Ediki.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommandHandler(
    INotificationRepository notificationRepository,
    ICurrentUserService currentUserService) : IRequestHandler<CreateNotificationCommand, Result<NotificationDto>>
{
    public async Task<Result<NotificationDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var createdByUserId = currentUserService.UserId;

            var notification = new Notification
            {
                UserId = request.UserId,
                Type = request.Type,
                Priority = request.Priority,
                Title = request.Title,
                Message = request.Message,
                ActionUrl = request.ActionUrl,
                RelatedEntityId = request.RelatedEntityId,
                RelatedEntityType = request.RelatedEntityType,
                MentionedUserIds = request.MentionedUserIds,
                CreatedByUserId = createdByUserId
            };

            var createdNotification = await notificationRepository.CreateAsync(notification);

            var notificationDto = new NotificationDto
            {
                Id = createdNotification.Id,
                UserId = createdNotification.UserId,
                Type = createdNotification.Type,
                Priority = createdNotification.Priority,
                Title = createdNotification.Title,
                Message = createdNotification.Message,
                ActionUrl = createdNotification.ActionUrl,
                RelatedEntityId = createdNotification.RelatedEntityId,
                RelatedEntityType = createdNotification.RelatedEntityType,
                IsRead = createdNotification.IsRead,
                MentionedUserIds = createdNotification.MentionedUserIds,
                CreatedByUserId = createdNotification.CreatedByUserId,
                CreatedByUserName = createdNotification.CreatedByUser?.UserName ?? string.Empty,
                CreatedAt = createdNotification.CreatedAt,
                ReadAt = createdNotification.ReadAt
            };

            return Result<NotificationDto>.Success(notificationDto);
        }
        catch (Exception ex)
        {
            return Result<NotificationDto>.Failure(ex.Message);
        }
    }
} 