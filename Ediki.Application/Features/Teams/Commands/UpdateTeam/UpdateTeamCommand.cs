using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommand : IRequest<Result<TeamDto>>
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public int? MaxMembers { get; set; }
    public bool? IsComplete { get; set; }
    public string? TeamLead { get; set; }

    public UpdateTeamCommand(string id)
    {
        Id = id;
    }
}