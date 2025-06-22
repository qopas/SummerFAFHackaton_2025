using MediatR;
using Ediki.Domain.Common;
using Ediki.Application.Features.ProjectMembers.DTOs;

namespace Ediki.Application.Features.ProjectMembers.Queries.GetProjectMembers;

public class GetProjectMembersQuery : IRequest<Result<IEnumerable<ProjectMemberDto>>>
{
    public string ProjectId { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public bool? IsProjectLead { get; set; }
} 