using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using Ediki.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Enums;

namespace Ediki.Application.Features.Teams.Commands.InviteUser;

public class InviteUserCommandHandler : IRequestHandler<InviteUserCommand, Result<TeamMemberDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationRepository _notificationRepository;

    public InviteUserCommandHandler(
        ITeamRepository teamRepository,
        ITeamMemberRepository teamMemberRepository,
        UserManager<ApplicationUser> userManager,
        INotificationRepository notificationRepository)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _userManager = userManager;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<TeamMemberDto>> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        // Check if team exists
        var team = await _teamRepository.GetByIdAsync(request.TeamId);
        if (team == null)
        {
            return Result<TeamMemberDto>.Failure("Team not found");
        }

        // Check if user exists
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result<TeamMemberDto>.Failure("User not found");
        }

        // Check if user is already a member or has pending invitation
        var existingMember = await _teamMemberRepository.GetByTeamAndUserAsync(request.TeamId, request.UserId);
        if (existingMember != null)
        {
            if (existingMember.IsActive)
            {
                return Result<TeamMemberDto>.Failure("User is already a member of this team");
            }
            if (existingMember.InvitedAt != null && existingMember.AcceptedAt == null)
            {
                return Result<TeamMemberDto>.Failure("User already has a pending invitation to this team");
            }
        }

        // Check if team has reached max capacity
        var currentMemberCount = await _teamMemberRepository.GetActiveTeamMemberCountAsync(request.TeamId);
        if (currentMemberCount >= team.MaxMembers)
        {
            return Result<TeamMemberDto>.Failure("Team has reached maximum capacity");
        }

        // Create invitation
        var teamMember = TeamMember.CreateInvitation(
            request.TeamId,
            request.UserId,
            request.Role,
            request.InvitedBy);

        await _teamMemberRepository.AddAsync(teamMember);

        // Get inviter information
        var inviter = await _userManager.FindByIdAsync(request.InvitedBy);
        var inviterName = inviter != null ? $"{inviter.FirstName} {inviter.LastName}".Trim() : "Unknown";

        // Create notification
        var notification = new Notification
        {
            UserId = request.UserId,
            Type = NotificationType.TeamInvitation,
            Priority = NotificationPriority.High,
            Title = "Team Invitation",
            Message = $"{inviterName} invited you to join the team '{team.Name}' as {request.Role}",
            ActionUrl = $"/teams/{team.Id}",
            RelatedEntityId = team.Id,
            RelatedEntityType = "Team",
            CreatedByUserId = request.InvitedBy
        };

        await _notificationRepository.CreateAsync(notification);

        var teamMemberDto = new TeamMemberDto
        {
            Id = teamMember.Id,
            TeamId = teamMember.TeamId,
            UserId = teamMember.UserId,
            UserName = user.UserName ?? "Unknown",
            UserEmail = user.Email ?? "Unknown",
            Role = teamMember.Role,
            JoinedAt = teamMember.JoinedAt,
            Progress = teamMember.Progress,
            IsActive = teamMember.IsActive,
            InvitedBy = teamMember.InvitedBy,
            InvitedAt = teamMember.InvitedAt,
            AcceptedAt = teamMember.AcceptedAt
        };

        return Result<TeamMemberDto>.Success(teamMemberDto);
    }
}