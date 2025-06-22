using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.ProjectMembers.Commands.AcceptInvitation;

public class AcceptInvitationCommand : IRequest<Result<ProjectMemberDto>>
{
    public string InvitationId { get; set; } = string.Empty;
} 