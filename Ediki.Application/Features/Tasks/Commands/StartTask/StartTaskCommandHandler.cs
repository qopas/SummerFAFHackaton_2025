using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Tasks.Commands.StartTask;

public class StartTaskCommandHandler(
    ITaskRepository taskRepository,
    ICurrentUserService currentUserService) : IRequestHandler<StartTaskCommand, Result<TaskDto>>
{
    public async Task<Result<TaskDto>> Handle(StartTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
            
            var task = await taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                return Result<TaskDto>.Failure($"Task with ID {request.TaskId} not found.");

            if (!string.IsNullOrEmpty(task.AssigneeId) && task.AssigneeId != userId)
                return Result<TaskDto>.Failure("You can only start tasks assigned to you.");

            if (string.IsNullOrEmpty(task.AssigneeId))
            {
                task.AssigneeId = userId;
            }

            task.Status = TaskStatus.InProgress;
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
} 