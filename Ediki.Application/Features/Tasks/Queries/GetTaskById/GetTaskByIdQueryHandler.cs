using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task == null)
            return null;

        return new TaskDto
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
        };
    }
} 