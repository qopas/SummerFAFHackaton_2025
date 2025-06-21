using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.UpdateMemberRole;

public class UpdateMemberRoleCommand : IRequest<Result<TeamMemberDto>>
{
    public string TeamId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public UpdateMemberRoleCommand(string teamId, string userId, string role)
    {
        TeamId = teamId;
        UserId = userId;
        Role = role;
    }
}