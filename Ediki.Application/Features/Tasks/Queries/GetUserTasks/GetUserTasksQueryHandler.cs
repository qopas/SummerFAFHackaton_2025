using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetUserTasks;

public class GetUserTasksQueryHandler : IRequestHandler<GetUserTasksQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetUserTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskDto>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetUserTasksAsync(request.UserId);

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