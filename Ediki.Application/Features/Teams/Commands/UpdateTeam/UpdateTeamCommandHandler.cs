using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Domain.Entities;

namespace Ediki.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result<TeamDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateTeamCommandHandler(
        ITeamRepository teamRepository,
        ITeamMemberRepository teamMemberRepository,
        UserManager<ApplicationUser> userManager)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _userManager = userManager;
    }

    public async Task<Result<TeamDto>> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(request.Id);
        if (team == null)
        {
            return Result<TeamDto>.Failure("Team not found");
        }

        // Update team properties if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            team.UpdateName(request.Name);
        }

        if (request.MaxMembers.HasValue)
        {
            team.UpdateMaxMembers(request.MaxMembers.Value);
        }

        if (request.IsComplete.HasValue)
        {
            team.SetComplete(request.IsComplete.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.TeamLead))
        {
            team.SetTeamLead(request.TeamLead);
        }

        await _teamRepository.UpdateAsync(team);

        // Get updated team data
        var teamMembers = await _teamMemberRepository.GetByTeamIdAsync(team.Id);
        var memberCount = await _teamMemberRepository.GetActiveTeamMemberCountAsync(team.Id);

        var memberDtos = new List<TeamMemberDto>();
        foreach (var member in teamMembers)
        {
            var user = await _userManager.FindByIdAsync(member.UserId);
            memberDtos.Add(new TeamMemberDto
            {
                Id = member.Id,
                TeamId = member.TeamId,
                UserId = member.UserId,
                UserName = user?.UserName ?? "Unknown",
                UserEmail = user?.Email ?? "Unknown",
                Role = member.Role,
                JoinedAt = member.JoinedAt,
                Progress = member.Progress,
                IsActive = member.IsActive,
                InvitedBy = member.InvitedBy,
                InvitedAt = member.InvitedAt,
                AcceptedAt = member.AcceptedAt
            });
        }

        var teamDto = new TeamDto
        {
            Id = team.Id,
            ProjectId = team.ProjectId,
            Name = team.Name,
            MaxMembers = team.MaxMembers,
            IsComplete = team.IsComplete,
            InviteCode = team.InviteCode,
            TeamLead = team.TeamLead,
            CurrentMemberCount = memberCount,
            CreatedAt = team.CreatedAt,
            UpdatedAt = team.UpdatedAt,
            Members = memberDtos
        };

        return Result<TeamDto>.Success(teamDto);
    }
}