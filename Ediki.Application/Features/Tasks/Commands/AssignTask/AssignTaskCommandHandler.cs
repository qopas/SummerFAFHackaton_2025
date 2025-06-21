using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.AssignTask;

public class AssignTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<AssignTaskCommand, Result<TaskDto>>
{
    public async System.Threading.Tasks.Task<Result<TaskDto>> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                return Result<TaskDto>.Failure($"Task with ID {request.TaskId} not found.");

            task.AssigneeId = request.AssigneeId;
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