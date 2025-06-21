using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.InviteUser;

public class InviteUserCommand : IRequest<Result<TeamMemberDto>>
{
    public string TeamId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string InvitedBy { get; set; } = string.Empty;

    public InviteUserCommand(string teamId, string userId, string role, string invitedBy)
    {
        TeamId = teamId;
        UserId = userId;
        Role = role;
        InvitedBy = invitedBy;
    }
}