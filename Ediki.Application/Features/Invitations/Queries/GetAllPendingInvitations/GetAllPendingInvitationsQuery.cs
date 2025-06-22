using Ediki.Application.Features.Invitations.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Invitations.Queries.GetAllPendingInvitations;

public class GetAllPendingInvitationsQuery : IRequest<Result<List<InvitationDto>>>
{
    public string UserId { get; set; } = string.Empty;
} 