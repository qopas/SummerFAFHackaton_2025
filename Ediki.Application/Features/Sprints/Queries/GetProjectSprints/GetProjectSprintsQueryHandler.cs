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
            Order = sprint.Order,
            Goals = sprint.Goals,
            Deliverables = sprint.Deliverables,
            CreatedAt = sprint.CreatedAt,
            UpdatedAt = sprint.UpdatedAt,
            TaskCount = sprint.Tasks?.Count ?? 0,
            CompletedTaskCount = sprint.Tasks?.Count(t => t.Status == Ediki.Domain.Enums.TaskStatus.Completed) ?? 0
        }).ToList();
    }
} 