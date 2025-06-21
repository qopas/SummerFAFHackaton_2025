using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommand : IRequest<Result<TeamDto>>
{
    public string ProjectId { get; set; } = string.Empty;
    public string? Name { get; set; }
    public int MaxMembers { get; set; } = 6;
    public string? TeamLead { get; set; }
    public bool GenerateInviteCode { get; set; } = true;

    public CreateTeamCommand(string projectId)
    {
        ProjectId = projectId;
    }
} 