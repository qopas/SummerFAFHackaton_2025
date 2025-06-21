using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Teams.Commands.UpdateMemberRole;

namespace SummerFAFHackaton_2025.DTOs.In.Teams;

public class UpdateMemberRoleRequest : IRequestIn<UpdateMemberRoleCommand>
{
    public string Role { get; set; } = string.Empty;

    public UpdateMemberRoleCommand Convert()
    {
        throw new NotImplementedException("This method should be called with team ID and user ID parameters");
    }

    public UpdateMemberRoleCommand Convert(string teamId, string userId)
    {
        return new UpdateMemberRoleCommand(teamId, userId, Role);
    }
}