using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using Ediki.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ediki.Application.Features.Teams.Commands.JoinTeam;

public class JoinTeamCommandHandler : IRequestHandler<JoinTeamCommand, Result<TeamMemberDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public JoinTeamCommandHandler(
        ITeamRepository teamRepository,
        ITeamMemberRepository teamMemberRepository,
        UserManager<ApplicationUser> userManager)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _userManager = userManager;
    }

    public async Task<Result<TeamMemberDto>> Handle(JoinTeamCommand request, CancellationToken cancellationToken)
    {
        // Check if user exists
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result<TeamMemberDto>.Failure("User not found");
        }

        // Get team by ID or invite code
        Team? team = null;
        if (!string.IsNullOrWhiteSpace(request.InviteCode))
        {
            team = await _teamRepository.GetByInviteCodeAsync(request.InviteCode);
            if (team == null)
            {
                return Result<TeamMemberDto>.Failure("Invalid invite code");
            }
        }
        else
        {
            team = await _teamRepository.GetByIdAsync(request.TeamId);
            if (team == null)
            {
                return Result<TeamMemberDto>.Failure("Team not found");
            }
        }

        // Check if user already has a pending invitation
        var existingMember = await _teamMemberRepository.GetByTeamAndUserAsync(team.Id, request.UserId);
        if (existingMember != null)
        {
            if (existingMember.IsActive)
            {
                return Result<TeamMemberDto>.Failure("User is already a member of this team");
            }
            
            // If there's a pending invitation, accept it
            if (existingMember.InvitedAt != null && existingMember.AcceptedAt == null)
            {
                existingMember.AcceptInvitation();
                await _teamMemberRepository.UpdateAsync(existingMember);

                var memberDto = new TeamMemberDto
                {
                    Id = existingMember.Id,
                    TeamId = existingMember.TeamId,
                    UserId = existingMember.UserId,
                    UserName = user.UserName ?? "Unknown",
                    UserEmail = user.Email ?? "Unknown",
                    Role = existingMember.Role,
                    JoinedAt = existingMember.JoinedAt,
                    Progress = existingMember.Progress,
                    IsActive = existingMember.IsActive,
                    InvitedBy = existingMember.InvitedBy,
                    InvitedAt = existingMember.InvitedAt,
                    AcceptedAt = existingMember.AcceptedAt
                };

                return Result<TeamMemberDto>.Success(memberDto);
            }
        }

        // Check if team has reached max capacity
        var currentMemberCount = await _teamMemberRepository.GetActiveTeamMemberCountAsync(team.Id);
        if (currentMemberCount >= team.MaxMembers)
        {
            return Result<TeamMemberDto>.Failure("Team has reached maximum capacity");
        }

        // Create new team member (direct join)
        var teamMember = TeamMember.Create(
            team.Id,
            request.UserId,
            request.Role);

        await _teamMemberRepository.AddAsync(teamMember);

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