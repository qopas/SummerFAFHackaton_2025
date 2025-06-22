using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;
using TaskPriority = Ediki.Domain.Enums.TaskPriority;
using DomainTask = Ediki.Domain.Entities.Task;

namespace Ediki.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<UpdateTaskCommand, Result<TaskDto>>
{
    public async System.Threading.Tasks.Task<Result<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await taskRepository.GetByIdAsync(request.Id);
            if (task == null)
                return Result<TaskDto>.Failure($"Task with ID {request.Id} not found.");

            if (request.Title != null)
            {
                task.Title = request.Title;
            }

            if (request.Description != null)
            {
                task.Description = request.Description;
            }

            if (request.AssigneeId != null)
            {
                task.AssigneeId = request.AssigneeId;
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                var parsedStatus = ParseTaskStatus(request.Status);
                if (parsedStatus.HasValue)
                {
                    task.Status = parsedStatus.Value;
                }
            }

            if (!string.IsNullOrEmpty(request.Priority))
            {
                var parsedPriority = ParseTaskPriority(request.Priority);
                if (parsedPriority.HasValue)
                {
                    task.Priority = parsedPriority.Value;
                }
            }

            if (request.EstimatedHours.HasValue)
            {
                task.EstimatedHours = request.EstimatedHours;
            }

            if (request.ActualHours.HasValue)
            {
                task.ActualHours = request.ActualHours;
            }

            if (request.Tags != null)
            {
                task.Tags = request.Tags;
            }

            if (request.Dependencies != null)
            {
                task.Dependencies = request.Dependencies;
            }

            if (request.DueDate.HasValue)
            {
                task.DueDate = request.DueDate;
            }

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

    private static TaskStatus? ParseTaskStatus(string? status)
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

    private static TaskPriority? ParseTaskPriority(string? priority)
    {
        if (string.IsNullOrEmpty(priority))
            return null;

        return priority.ToLower() switch
        {
            "low" => TaskPriority.Low,
            "medium" => TaskPriority.Medium,
            "high" => TaskPriority.High,
            "urgent" => TaskPriority.Urgent,
            _ => int.TryParse(priority, out var priorityInt) ? (TaskPriority)priorityInt : 
                 Enum.TryParse<TaskPriority>(priority, true, out var priorityEnum) ? priorityEnum : null
        };
    }
} 