using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.ProjectMembers.Queries.GetPendingInvitations;

public class GetPendingInvitationsQuery : IRequest<Result<List<ProjectMemberDto>>>
{
    public string UserId { get; set; } = string.Empty;
} 