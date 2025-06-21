using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using Ediki.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ediki.Application.Features.Teams.Commands.UpdateMemberRole;

public class UpdateMemberRoleCommandHandler : IRequestHandler<UpdateMemberRoleCommand, Result<TeamMemberDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateMemberRoleCommandHandler(
        ITeamRepository teamRepository,
        ITeamMemberRepository teamMemberRepository,
        UserManager<ApplicationUser> userManager)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _userManager = userManager;
    }

    public async Task<Result<TeamMemberDto>> Handle(UpdateMemberRoleCommand request, CancellationToken cancellationToken)
    {
        // Check if team exists
        var team = await _teamRepository.GetByIdAsync(request.TeamId);
        if (team == null)
        {
            return Result<TeamMemberDto>.Failure("Team not found");
        }

        // Check if team member exists
        var teamMember = await _teamMemberRepository.GetByTeamAndUserAsync(request.TeamId, request.UserId);
        if (teamMember == null)
        {
            return Result<TeamMemberDto>.Failure("Team member not found");
        }

        // Get user details
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result<TeamMemberDto>.Failure("User not found");
        }

        // Update the role
        teamMember.UpdateRole(request.Role);
        await _teamMemberRepository.UpdateAsync(teamMember);

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