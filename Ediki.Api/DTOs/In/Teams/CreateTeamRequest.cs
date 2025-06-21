using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Teams.Commands.CreateTeam;

namespace SummerFAFHackaton_2025.DTOs.In.Teams;

public class CreateTeamRequest : IRequestIn<CreateTeamCommand>
{
    public string? Name { get; set; }
    public int MaxMembers { get; set; } = 6;
    public string? TeamLead { get; set; }
    public bool GenerateInviteCode { get; set; } = true;

    public CreateTeamCommand Convert()
    {
        throw new NotImplementedException("This method should be called with project ID parameter");
    }

    public CreateTeamCommand Convert(string projectId)
    {
        return new CreateTeamCommand(projectId)
        {
            Name = Name,
            MaxMembers = MaxMembers,
            TeamLead = TeamLead,
            GenerateInviteCode = GenerateInviteCode
        };
    }
} 