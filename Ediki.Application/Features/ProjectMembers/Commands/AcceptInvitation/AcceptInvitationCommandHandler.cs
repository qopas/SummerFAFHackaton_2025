using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Domain.Enums;

namespace Ediki.Application.Features.ProjectMembers.Commands.AcceptInvitation;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result<ProjectMemberDto>>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationRepository _notificationRepository;

    public AcceptInvitationCommandHandler(
        IProjectMemberRepository projectMemberRepository,
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager,
        INotificationRepository notificationRepository)
    {
        _projectMemberRepository = projectMemberRepository;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<ProjectMemberDto>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

            // Get the invitation
            var invitation = await _projectMemberRepository.GetByIdAsync(request.InvitationId);
            if (invitation == null)
            {
                return Result<ProjectMemberDto>.Failure("Invitation not found");
            }

            // Verify this invitation belongs to the current user
            if (invitation.UserId != currentUserId)
            {
                return Result<ProjectMemberDto>.Failure("You can only accept your own invitations");
            }

            // Check if invitation is still pending
            if (invitation.AcceptedAt != null)
            {
                return Result<ProjectMemberDto>.Failure("Invitation has already been accepted");
            }

            if (invitation.InvitedAt == null)
            {
                return Result<ProjectMemberDto>.Failure("This is not a valid invitation");
            }

            // Accept the invitation
            invitation.AcceptInvitation();
            await _projectMemberRepository.UpdateAsync(invitation);

            // Get user information for the response
            var user = await _userManager.FindByIdAsync(currentUserId);
            var inviter = invitation.InvitedBy != null ? await _userManager.FindByIdAsync(invitation.InvitedBy) : null;

            // Create notification for the inviter
            if (invitation.InvitedBy != null && invitation.Project != null)
            {
                var userName = user != null ? $"{user.FirstName} {user.LastName}".Trim() : "Someone";
                var notification = new Notification
                {
                    UserId = invitation.InvitedBy,
                    Type = NotificationType.ProjectInvitation,
                    Priority = NotificationPriority.Normal,
                    Title = "Invitation Accepted",
                    Message = $"{userName} accepted your invitation to join the project '{invitation.Project.Title}'",
                    ActionUrl = $"/projects/{invitation.ProjectId}",
                    RelatedEntityId = invitation.ProjectId,
                    RelatedEntityType = "Project",
                    CreatedByUserId = currentUserId
                };

                await _notificationRepository.CreateAsync(notification);
            }

            var projectMemberDto = new ProjectMemberDto
            {
                Id = invitation.Id,
                ProjectId = invitation.ProjectId,
                UserId = invitation.UserId,
                UserName = user?.UserName ?? string.Empty,
                UserEmail = user?.Email ?? string.Empty,
                UserFirstName = user?.FirstName ?? string.Empty,
                UserLastName = user?.LastName ?? string.Empty,
                UserPreferredRole = user?.PreferredRole ?? Domain.Enums.PreferredRole.NotSet,
                Role = invitation.Role,
                JoinedAt = invitation.JoinedAt,
                Progress = invitation.Progress,
                IsActive = invitation.IsActive,
                InvitedBy = invitation.InvitedBy,
                InvitedByName = inviter != null ? $"{inviter.FirstName} {inviter.LastName}".Trim() : null,
                InvitedAt = invitation.InvitedAt,
                AcceptedAt = invitation.AcceptedAt,
                IsProjectLead = invitation.IsProjectLead
            };

            return Result<ProjectMemberDto>.Success(projectMemberDto);
        }
        catch (Exception ex)
        {
            return Result<ProjectMemberDto>.Failure($"Error accepting invitation: {ex.Message}");
        }
    }
} 