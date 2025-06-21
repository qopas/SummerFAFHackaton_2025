using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByIdQuery, Result<TaskDto?>>
{
    public async System.Threading.Tasks.Task<Result<TaskDto?>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await taskRepository.GetByIdAsync(request.Id);
            if (task == null)
                return Result<TaskDto?>.Success(null);

            var taskDto = new TaskDto
            {
                Id = task.Id,
                SprintId = task.SprintId,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                AssigneeId = task.AssigneeId,
                AssigneeName = task.Assignee?.UserName ?? string.Empty,
                AssigneeEmail = task.Assignee?.Email ?? string.Empty,
                Status = task.Status,
                Priority = task.Priority,
                EstimatedHours = task.EstimatedHours,
                ActualHours = task.ActualHours,
                Tags = task.Tags,
                Dependencies = task.Dependencies,
                DueDate = task.DueDate,
                CompletedAt = task.CompletedAt,
                CreatedBy = task.CreatedBy,
                CreatedByName = task.CreatedByUser?.UserName ?? string.Empty,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };

            return Result<TaskDto?>.Success(taskDto);
        }
        catch (Exception ex)
        {
            return Result<TaskDto?>.Failure(ex.Message);
        }
    }
} 