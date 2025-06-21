using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Teams.Commands.UpdateTeam;

namespace SummerFAFHackaton_2025.DTOs.In.Teams;

public class UpdateTeamRequest : IRequestIn<UpdateTeamCommand>
{
    public string? Name { get; set; }
    public int? MaxMembers { get; set; }
    public bool? IsComplete { get; set; }
    public string? TeamLead { get; set; }

    public UpdateTeamCommand Convert()
    {
        throw new NotImplementedException("This method should be called with team ID parameter");
    }

    public UpdateTeamCommand Convert(string teamId)
    {
        return new UpdateTeamCommand(teamId)
        {
            Name = Name,
            MaxMembers = MaxMembers,
            IsComplete = IsComplete,
            TeamLead = TeamLead
        };
    }
}