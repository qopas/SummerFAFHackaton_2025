using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommandHandler(INotificationRepository notificationRepository) : IRequestHandler<MarkAsReadCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await notificationRepository.MarkAsReadAsync(request.NotificationId);
            
            if (!success)
                return Result<bool>.Failure("Notification not found");

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }
} 