using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Notifications.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ediki.Application.Features.Notifications.Commands.CreateMention;

public class CreateMentionCommandHandler(
    INotificationRepository notificationRepository,
    ICurrentUserService currentUserService,
    UserManager<ApplicationUser> userManager) : IRequestHandler<CreateMentionCommand, Result<List<NotificationDto>>>
{
    public async Task<Result<List<NotificationDto>>> Handle(CreateMentionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var createdByUserId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
            var createdByUser = await userManager.FindByIdAsync(createdByUserId);
            if (createdByUser == null)
                return Result<List<NotificationDto>>.Failure("User not found");

            var createdNotifications = new List<NotificationDto>();

            foreach (var mentionedUserId in request.MentionedUserIds.Distinct())
            {
                if (mentionedUserId == createdByUserId)
                    continue;

                var mentionedUser = await userManager.FindByIdAsync(mentionedUserId);
                if (mentionedUser == null || mentionedUser.IsDeleted)
                    continue;

                var title = $"You were mentioned by {createdByUser.FirstName} {createdByUser.LastName}";
                var message = string.IsNullOrEmpty(request.Context) 
                    ? request.Message 
                    : $"In {request.Context}: {request.Message}";

                var notification = new Notification
                {
                    UserId = mentionedUserId,
                    Type = NotificationType.Mention,
                    Priority = request.Priority,
                    Title = title,
                    Message = message,
                    ActionUrl = request.ActionUrl,
                    RelatedEntityId = request.RelatedEntityId,
                    RelatedEntityType = request.RelatedEntityType,
                    MentionedUserIds = new List<string> { mentionedUserId },
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
                    CreatedByUserName = $"{createdByUser.FirstName} {createdByUser.LastName}",
                    CreatedAt = createdNotification.CreatedAt,
                    ReadAt = createdNotification.ReadAt
                };

                createdNotifications.Add(notificationDto);
            }

            return Result<List<NotificationDto>>.Success(createdNotifications);
        }
        catch (Exception ex)
        {
            return Result<List<NotificationDto>>.Failure(ex.Message);
        }
    }
} 