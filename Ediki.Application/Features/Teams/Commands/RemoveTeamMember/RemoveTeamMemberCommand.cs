using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.RemoveTeamMember;

public class RemoveTeamMemberCommand : IRequest<Result<bool>>
{
    public string TeamId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public RemoveTeamMemberCommand(string teamId, string userId)
    {
        TeamId = teamId;
        UserId = userId;
    }
}