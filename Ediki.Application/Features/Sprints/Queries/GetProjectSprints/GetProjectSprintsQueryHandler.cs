using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using MediatR;

namespace Ediki.Application.Features.Sprints.Queries.GetProjectSprints;

public class GetProjectSprintsQueryHandler(ISprintRepository sprintRepository) : IRequestHandler<GetProjectSprintsQuery, IEnumerable<SprintDto>>
{
    public async System.Threading.Tasks.Task<IEnumerable<SprintDto>> Handle(GetProjectSprintsQuery request, CancellationToken cancellationToken)
    {
        var sprints = await sprintRepository.GetByProjectIdAsync(request.ProjectId);
        
        return sprints.Select(sprint => new SprintDto
        {
            Id = sprint.Id,
            ProjectId = sprint.ProjectId,
            Name = sprint.Name,
            Description = sprint.Description,
            StartDate = sprint.StartDate,
            EndDate = sprint.EndDate,
            Status = sprint.Status,
            Goals = sprint.Goals,
            CreatedBy = sprint.CreatedBy,
            CreatedByName = sprint.CreatedByUser?.UserName ?? string.Empty,
            CreatedAt = sprint.CreatedAt,
            UpdatedAt = sprint.UpdatedAt
        }).ToList();
    }
} 