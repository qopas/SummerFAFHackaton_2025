using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Enums;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Sprints.Queries.GetSprintById;

public class GetSprintByIdQueryHandler(ISprintRepository sprintRepository) : IRequestHandler<GetSprintByIdQuery, SprintDto?>
{
    public async System.Threading.Tasks.Task<SprintDto?> Handle(GetSprintByIdQuery request, CancellationToken cancellationToken)
    {
        var sprint = await sprintRepository.GetByIdAsync(request.Id);
        if (sprint == null)
            return null;

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
    }
} 