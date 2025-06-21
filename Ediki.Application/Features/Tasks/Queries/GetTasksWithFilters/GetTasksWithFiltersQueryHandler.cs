using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetTasksWithFilters;

public class GetTasksWithFiltersQueryHandler : IRequestHandler<GetTasksWithFiltersQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksWithFiltersQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksWithFiltersQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetTasksWithFiltersAsync(
            request.ProjectId,
            request.SprintId,
            request.AssigneeId,
            request.Status,
            request.Priority,
            request.DueDateFrom,
            request.DueDateTo);

        return tasks.Select(task => new TaskDto
        {
            Id = task.Id,
            SprintId = task.SprintId,
            ProjectId = task.ProjectId,
            Title = task.Title,
            Description = task.Description,
            AssigneeId = task.AssigneeId,
            AssigneeName = task.Assignee?.UserName,
            AssigneeEmail = task.Assignee?.Email,
            Status = task.Status,
            Priority = task.Priority,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            Tags = task.Tags,
            Dependencies = task.Dependencies,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            CreatedBy = task.CreatedBy,
            CreatedByName = task.CreatedByUser?.UserName ?? string.Empty,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        }).ToList();
    }
} 