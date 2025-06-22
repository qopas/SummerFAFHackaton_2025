using Ediki.Application.Features.Teams.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using Ediki.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Enums;

namespace Ediki.Application.Features.Teams.Commands.AcceptTeamInvitation;

public class AcceptTeamInvitationCommandHandler : IRequestHandler<AcceptTeamInvitationCommand, Result<TeamMemberDto>>
{
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationRepository _notificationRepository;

    public AcceptTeamInvitationCommandHandler(
        ITeamMemberRepository teamMemberRepository,
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager,
        INotificationRepository notificationRepository)
    {
        _teamMemberRepository = teamMemberRepository;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<TeamMemberDto>> Handle(AcceptTeamInvitationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

            // Get the invitation
            var invitation = await _teamMemberRepository.GetByIdAsync(request.InvitationId);
            if (invitation == null)
            {
                return Result<TeamMemberDto>.Failure("Invitation not found");
            }

            // Verify this invitation belongs to the current user
            if (invitation.UserId != currentUserId)
            {
                return Result<TeamMemberDto>.Failure("You can only accept your own invitations");
            }

            // Check if invitation is still pending
            if (invitation.AcceptedAt != null)
            {
                return Result<TeamMemberDto>.Failure("Invitation has already been accepted");
            }

            if (invitation.InvitedAt == null)
            {
                return Result<TeamMemberDto>.Failure("This is not a valid invitation");
            }

            // Accept the invitation
            invitation.AcceptInvitation();
            await _teamMemberRepository.UpdateAsync(invitation);

            // Get user information for the response
            var user = await _userManager.FindByIdAsync(currentUserId);
            var inviter = invitation.InvitedBy != null ? await _userManager.FindByIdAsync(invitation.InvitedBy) : null;

            // Create notification for the inviter
            if (invitation.InvitedBy != null)
            {
                var userName = user != null ? $"{user.FirstName} {user.LastName}".Trim() : "Someone";
                var notification = new Notification
                {
                    UserId = invitation.InvitedBy,
                    Type = NotificationType.TeamInvitation,
                    Priority = NotificationPriority.Normal,
                    Title = "Team Invitation Accepted",
                    Message = $"{userName} accepted your invitation to join the team",
                    ActionUrl = $"/teams/{invitation.TeamId}",
                    RelatedEntityId = invitation.TeamId,
                    RelatedEntityType = "Team",
                    CreatedByUserId = currentUserId
                };

                await _notificationRepository.CreateAsync(notification);
            }

            var teamMemberDto = new TeamMemberDto
            {
                Id = invitation.Id,
                TeamId = invitation.TeamId,
                UserId = invitation.UserId,
                UserName = user?.UserName ?? string.Empty,
                UserEmail = user?.Email ?? string.Empty,
                Role = invitation.Role,
                JoinedAt = invitation.JoinedAt,
                Progress = invitation.Progress,
                IsActive = invitation.IsActive,
                InvitedBy = invitation.InvitedBy,
                InvitedAt = invitation.InvitedAt,
                AcceptedAt = invitation.AcceptedAt
            };

            return Result<TeamMemberDto>.Success(teamMemberDto);
        }
        catch (Exception ex)
        {
            return Result<TeamMemberDto>.Failure($"Error accepting team invitation: {ex.Message}");
        }
    }
} 