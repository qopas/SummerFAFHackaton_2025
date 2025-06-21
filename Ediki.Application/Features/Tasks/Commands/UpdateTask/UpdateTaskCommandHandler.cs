using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using MediatR;
using DomainTask = Ediki.Domain.Entities.Task;

namespace Ediki.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task == null)
            throw new KeyNotFoundException($"Task with ID {request.Id} not found.");

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