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

            task.Title = request.Title;
            task.Description = request.Description;
            task.AssigneeId = request.AssigneeId;
            task.Status = request.Status;
            task.Priority = request.Priority;
            task.EstimatedHours = request.EstimatedHours;
            task.ActualHours = request.ActualHours;
            task.Tags = request.Tags;
            task.Dependencies = request.Dependencies;
            task.DueDate = request.DueDate;

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