using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.AssignTask;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public AssignTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task<TaskDto> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
            throw new KeyNotFoundException($"Task with ID {request.TaskId} not found.");

        task.AssigneeId = request.AssigneeId;
        var updatedTask = await _taskRepository.UpdateAsync(task);

        // Reload with navigation properties
        var taskWithDetails = await _taskRepository.GetByIdAsync(updatedTask.Id);

        return new TaskDto
        {
            Id = taskWithDetails!.Id,
            SprintId = taskWithDetails.SprintId,
            ProjectId = taskWithDetails.ProjectId,
            Title = taskWithDetails.Title,
            Description = taskWithDetails.Description,
            AssigneeId = taskWithDetails.AssigneeId,
            AssigneeName = taskWithDetails.Assignee?.UserName,
            AssigneeEmail = taskWithDetails.Assignee?.Email,
            Status = taskWithDetails.Status,
            Priority = taskWithDetails.Priority,
            EstimatedHours = taskWithDetails.EstimatedHours,
            ActualHours = taskWithDetails.ActualHours,
            Tags = taskWithDetails.Tags,
            Dependencies = taskWithDetails.Dependencies,
            DueDate = taskWithDetails.DueDate,
            CompletedAt = taskWithDetails.CompletedAt,
            CreatedBy = taskWithDetails.CreatedBy,
            CreatedByName = taskWithDetails.CreatedByUser?.UserName ?? string.Empty,
            CreatedAt = taskWithDetails.CreatedAt,
            UpdatedAt = taskWithDetails.UpdatedAt
        };
    }
} 