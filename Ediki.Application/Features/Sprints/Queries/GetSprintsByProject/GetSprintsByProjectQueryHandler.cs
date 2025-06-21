using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Sprints.Queries.GetSprintsByProject;

public class GetSprintsByProjectQueryHandler : IRequestHandler<GetSprintsByProjectQuery, IEnumerable<SprintDto>>
{
    private readonly ISprintRepository _sprintRepository;

    public GetSprintsByProjectQueryHandler(ISprintRepository sprintRepository)
    {
        _sprintRepository = sprintRepository;
    }

    public async System.Threading.Tasks.Task<IEnumerable<SprintDto>> Handle(GetSprintsByProjectQuery request, CancellationToken cancellationToken)
    {
        var sprints = await _sprintRepository.GetByProjectIdAsync(request.ProjectId);

        return sprints.Select(sprint =>
        {
            var taskCount = sprint.Tasks?.Count ?? 0;
            var completedTaskCount = sprint.Tasks?.Count(t => t.Status == TaskStatus.Completed) ?? 0;

            return new SprintDto
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
                TaskCount = taskCount,
                CompletedTaskCount = completedTaskCount
            };
        }).ToList();
    }
} 