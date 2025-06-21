using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.JoinTeam;

public class JoinTeamCommand : IRequest<Result<TeamMemberDto>>
{
    public string TeamId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string? InviteCode { get; set; }
    public string Role { get; set; } = "Member";

    public JoinTeamCommand(string teamId, string userId)
    {
        TeamId = teamId;
        UserId = userId;
    }
}