using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.AcceptTeamInvitation;

public class AcceptTeamInvitationCommand : IRequest<Result<TeamMemberDto>>
{
    public string InvitationId { get; set; } = string.Empty;
} 