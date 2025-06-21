using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Teams.Commands.JoinTeam;

namespace SummerFAFHackaton_2025.DTOs.In.Teams;

public class JoinTeamRequest : IRequestIn<JoinTeamCommand>
{
    public string UserId { get; set; } = string.Empty;
    public string? InviteCode { get; set; }
    public string Role { get; set; } = "Member";

    public JoinTeamCommand Convert()
    {
        throw new NotImplementedException("This method should be called with team ID parameter");
    }

    public JoinTeamCommand Convert(string teamId)
    {
        return new JoinTeamCommand(teamId, UserId)
        {
            InviteCode = InviteCode,
            Role = Role
        };
    }
}