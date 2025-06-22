using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler(ITaskRepository taskRepository) : IRequestHandler<UpdateTaskStatusCommand, Result<TaskDto>>
{
    public async System.Threading.Tasks.Task<Result<TaskDto>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                return Result<TaskDto>.Failure($"Task with ID {request.TaskId} not found.");

            // Parse status from string (supporting kebab-case)
            var taskStatus = ParseTaskStatus(request.Status);
            if (!taskStatus.HasValue)
                return Result<TaskDto>.Failure($"Invalid status value: {request.Status}");

            task.Status = taskStatus.Value;
            task.UpdatedAt = DateTime.UtcNow;
            var updatedTask = await taskRepository.UpdateAsync(task);

            var taskDto = new TaskDto
            {
                Id = updatedTask.Id,
                SprintId = updatedTask.SprintId,
                ProjectId = updatedTask.ProjectId,
                Title = updatedTask.Title,
                Description = updatedTask.Description,
                AssigneeId = updatedTask.AssigneeId,
                AssigneeName = updatedTask.Assignee?.UserName ?? string.Empty,
                AssigneeEmail = updatedTask.Assignee?.Email ?? string.Empty,
                Status = updatedTask.Status,
                Priority = updatedTask.Priority,
                EstimatedHours = updatedTask.EstimatedHours,
                ActualHours = updatedTask.ActualHours,
                Tags = updatedTask.Tags,
                Dependencies = updatedTask.Dependencies,
                DueDate = updatedTask.DueDate,
                CompletedAt = updatedTask.CompletedAt,
                CreatedBy = updatedTask.CreatedBy,
                CreatedByName = updatedTask.CreatedByUser?.UserName ?? string.Empty,
                CreatedAt = updatedTask.CreatedAt,
                UpdatedAt = updatedTask.UpdatedAt
            };

            return Result<TaskDto>.Success(taskDto);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
    }

    private static TaskStatus? ParseTaskStatus(string status)
    {
        if (string.IsNullOrEmpty(status))
            return null;

        return status.ToLower() switch
        {
            "todo" => TaskStatus.Todo,
            "in-progress" => TaskStatus.InProgress,
            "review" => TaskStatus.Review,
            "completed" => TaskStatus.Completed,
            _ => int.TryParse(status, out var statusInt) ? (TaskStatus)statusInt : 
                 Enum.TryParse<TaskStatus>(status, true, out var statusEnum) ? statusEnum : null
        };
    }
} 