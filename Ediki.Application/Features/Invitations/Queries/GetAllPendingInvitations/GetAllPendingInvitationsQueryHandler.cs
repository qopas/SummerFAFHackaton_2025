using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Invitations.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Invitations.Queries.GetAllPendingInvitations;

public class GetAllPendingInvitationsQueryHandler : IRequestHandler<GetAllPendingInvitationsQuery, Result<List<InvitationDto>>>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public GetAllPendingInvitationsQueryHandler(
        IProjectMemberRepository projectMemberRepository,
        ITeamMemberRepository teamMemberRepository)
    {
        _projectMemberRepository = projectMemberRepository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<Result<List<InvitationDto>>> Handle(GetAllPendingInvitationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allInvitations = new List<InvitationDto>();

            // Get project invitations
            var projectInvitations = await _projectMemberRepository.GetPendingInvitationsAsync(request.UserId);
            foreach (var invitation in projectInvitations)
            {
                allInvitations.Add(new InvitationDto
                {
                    Id = invitation.Id,
                    UserId = invitation.UserId,
                    RelatedEntityId = invitation.ProjectId,
                    RelatedEntityType = "Project",
                    RelatedEntityName = invitation.Project?.Title ?? string.Empty,
                    RelatedEntityDescription = invitation.Project?.Description ?? string.Empty,
                    Role = invitation.Role.ToString(),
                    JoinedAt = invitation.JoinedAt,
                    Progress = invitation.Progress,
                    IsActive = invitation.IsActive,
                    InvitedBy = invitation.InvitedBy,
                    InvitedByName = invitation.InvitedByUser != null ? $"{invitation.InvitedByUser.FirstName} {invitation.InvitedByUser.LastName}".Trim() : null,
                    InvitedAt = invitation.InvitedAt,
                    AcceptedAt = invitation.AcceptedAt,
                    IsProjectLead = invitation.IsProjectLead
                });
            }

            // Get team invitations
            var teamInvitations = await _teamMemberRepository.GetPendingInvitationsAsync(request.UserId);
            foreach (var invitation in teamInvitations)
            {
                allInvitations.Add(new InvitationDto
                {
                    Id = invitation.Id,
                    UserId = invitation.UserId,
                    RelatedEntityId = invitation.TeamId,
                    RelatedEntityType = "Team",
                    RelatedEntityName = "Team", // Teams don't have names in current structure
                    RelatedEntityDescription = string.Empty,
                    Role = invitation.Role,
                    JoinedAt = invitation.JoinedAt,
                    Progress = invitation.Progress,
                    IsActive = invitation.IsActive,
                    InvitedBy = invitation.InvitedBy,
                    InvitedByName = null, // TeamMember doesn't have InvitedByUser navigation
                    InvitedAt = invitation.InvitedAt,
                    AcceptedAt = invitation.AcceptedAt,
                    IsProjectLead = false
                });
            }

            // Sort by invitation date (newest first)
            allInvitations = allInvitations.OrderByDescending(i => i.InvitedAt).ToList();

            return Result<List<InvitationDto>>.Success(allInvitations);
        }
        catch (Exception ex)
        {
            return Result<List<InvitationDto>>.Failure($"Error retrieving pending invitations: {ex.Message}");
        }
    }
} 