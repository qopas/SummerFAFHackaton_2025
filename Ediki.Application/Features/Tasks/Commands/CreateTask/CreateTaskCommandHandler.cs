using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using MediatR;
using DomainTask = Ediki.Domain.Entities.Task;

namespace Ediki.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(
    ITaskRepository taskRepository,
    ICurrentUserService currentUserService) : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
{
    public async System.Threading.Tasks.Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

            var task = new DomainTask
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
                CreatedBy = userId
            };

            var createdTask = await taskRepository.CreateAsync(task);

            var taskDto = new TaskDto
            {
                Id = createdTask.Id,
                SprintId = createdTask.SprintId,
                ProjectId = createdTask.ProjectId,
                Title = createdTask.Title,
                Description = createdTask.Description,
                AssigneeId = createdTask.AssigneeId,
                AssigneeName = createdTask.Assignee?.UserName ?? string.Empty,
                AssigneeEmail = createdTask.Assignee?.Email ?? string.Empty,
                Status = createdTask.Status,
                Priority = createdTask.Priority,
                EstimatedHours = createdTask.EstimatedHours,
                ActualHours = createdTask.ActualHours,
                Tags = createdTask.Tags,
                Dependencies = createdTask.Dependencies,
                DueDate = createdTask.DueDate,
                CompletedAt = createdTask.CompletedAt,
                CreatedBy = createdTask.CreatedBy,
                CreatedByName = createdTask.CreatedByUser?.UserName ?? string.Empty,
                CreatedAt = createdTask.CreatedAt,
                UpdatedAt = createdTask.UpdatedAt
            };

            return Result<TaskDto>.Success(taskDto);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure(ex.Message);
        }
    }
} 