namespace Ediki.Domain.Enums;

public enum NotificationType
{
    TaskAssigned = 1,
    TaskStatusChanged = 2,
    TaskSubmittedForReview = 3,
    TaskCompleted = 4,
    ProjectInvitation = 5,
    ProjectMilestone = 6,
    Mention = 7,
    PeerReviewRequired = 8,
    General = 9
} 