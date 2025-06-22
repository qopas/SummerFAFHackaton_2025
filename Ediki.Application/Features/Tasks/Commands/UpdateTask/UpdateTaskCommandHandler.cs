using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;
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

            if (request.Status.HasValue)
            {
                task.Status = request.Status.Value;
            }

            if (request.Priority.HasValue)
            {
                task.Priority = request.Priority.Value;
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
} 