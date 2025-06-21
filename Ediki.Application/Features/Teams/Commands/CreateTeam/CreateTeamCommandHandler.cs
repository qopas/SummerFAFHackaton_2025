using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using Ediki.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ediki.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<TeamDto>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateTeamCommandHandler(
        ITeamRepository teamRepository,
        ITeamMemberRepository teamMemberRepository,
        IProjectRepository projectRepository,
        UserManager<ApplicationUser> userManager)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _projectRepository = projectRepository;
        _userManager = userManager;
    }

    public async Task<Result<TeamDto>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        // Check if project exists
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
        {
            return Result<TeamDto>.Failure("Project not found");
        }

        // Check if project already has a team
        var existingTeam = await _teamRepository.GetByProjectIdAsync(request.ProjectId);
        if (existingTeam != null)
        {
            return Result<TeamDto>.Failure("Project already has a team");
        }

        // Validate team lead if provided
        if (!string.IsNullOrWhiteSpace(request.TeamLead))
        {
            var teamLeadUser = await _userManager.FindByIdAsync(request.TeamLead);
            if (teamLeadUser == null)
            {
                return Result<TeamDto>.Failure("Team lead user not found");
            }
        }

        // Create the team
        var team = Team.Create(
            request.ProjectId,
            request.Name,
            request.MaxMembers,
            request.TeamLead);

        // Generate invite code if requested
        if (request.GenerateInviteCode)
        {
            team.GenerateInviteCode();
        }

        // Save the team
        await _teamRepository.AddAsync(team);

        // If team lead is specified, add them as a team member
        if (!string.IsNullOrWhiteSpace(request.TeamLead))
        {
            var teamLeadMember = TeamMember.Create(
                team.Id,
                request.TeamLead,
                "Team Lead");

            await _teamMemberRepository.AddAsync(teamLeadMember);
        }

        // Get team members for response
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