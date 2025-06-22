using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.ProjectMembers.Queries.GetPendingInvitations;

public class GetPendingInvitationsQueryHandler : IRequestHandler<GetPendingInvitationsQuery, Result<List<ProjectMemberDto>>>
{
    private readonly IProjectMemberRepository _projectMemberRepository;

    public GetPendingInvitationsQueryHandler(IProjectMemberRepository projectMemberRepository)
    {
        _projectMemberRepository = projectMemberRepository;
    }

    public async Task<Result<List<ProjectMemberDto>>> Handle(GetPendingInvitationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pendingInvitations = await _projectMemberRepository.GetPendingInvitationsAsync(request.UserId);

            var invitationDtos = pendingInvitations.Select(pm => new ProjectMemberDto
            {
                Id = pm.Id,
                ProjectId = pm.ProjectId,
                UserId = pm.UserId,
                UserName = string.Empty, // Not needed for invitations
                UserEmail = string.Empty, // Not needed for invitations
                UserFirstName = string.Empty,
                UserLastName = string.Empty,
                Role = pm.Role,
                JoinedAt = pm.JoinedAt,
                Progress = pm.Progress,
                IsActive = pm.IsActive,
                InvitedBy = pm.InvitedBy,
                InvitedByName = pm.InvitedByUser != null ? $"{pm.InvitedByUser.FirstName} {pm.InvitedByUser.LastName}".Trim() : null,
                InvitedAt = pm.InvitedAt,
                AcceptedAt = pm.AcceptedAt,
                IsProjectLead = pm.IsProjectLead,
                ProjectName = pm.Project?.Title ?? string.Empty,
                ProjectDescription = pm.Project?.Description ?? string.Empty
            }).ToList();

            return Result<List<ProjectMemberDto>>.Success(invitationDtos);
        }
        catch (Exception ex)
        {
            return Result<List<ProjectMemberDto>>.Failure($"Error retrieving pending invitations: {ex.Message}");
        }
    }
} 