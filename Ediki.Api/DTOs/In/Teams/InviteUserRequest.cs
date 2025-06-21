using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Teams.Commands.InviteUser;

namespace SummerFAFHackaton_2025.DTOs.In.Teams;

public class InviteUserRequest : IRequestIn<InviteUserCommand>
{
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = "Member";
    public string InvitedBy { get; set; } = string.Empty;

    public InviteUserCommand Convert()
    {
        throw new NotImplementedException("This method should be called with team ID parameter");
    }

    public InviteUserCommand Convert(string teamId)
    {
        return new InviteUserCommand(teamId, UserId, Role, InvitedBy);
    }
}