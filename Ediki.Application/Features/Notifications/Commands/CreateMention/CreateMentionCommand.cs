using Ediki.Application.Features.Notifications.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Notifications.Commands.CreateMention;

public class CreateMentionCommand : IRequest<Result<List<NotificationDto>>>
{
    public List<string> MentionedUserIds { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public string? Context { get; set; }
    public string? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public string? ActionUrl { get; set; }
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
} 