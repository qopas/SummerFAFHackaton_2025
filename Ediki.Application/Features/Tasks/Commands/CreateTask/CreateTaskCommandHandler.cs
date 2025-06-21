using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Application.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateTaskCommandHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("User not authenticated.");

        var task = new Domain.Entities.Task
        {
            SprintId = request.SprintId,
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            AssigneeId = request.AssigneeId,
            Status = request.Status,
            Priority = request.Priority,
            EstimatedHours = request.EstimatedHours,
            Tags = request.Tags,
            Dependencies = request.Dependencies,
            DueDate = request.DueDate,
            CreatedBy = currentUserId
        };

        var createdTask = await _taskRepository.CreateAsync(task);

        // Load the created task with navigation properties
        var taskWithDetails = await _taskRepository.GetByIdAsync(createdTask.Id);

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