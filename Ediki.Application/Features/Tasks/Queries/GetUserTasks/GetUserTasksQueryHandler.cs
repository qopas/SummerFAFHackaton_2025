using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetUserTasks;

public class GetUserTasksQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetUserTasksQuery, Result<IEnumerable<TaskDto>>>
{
    public async System.Threading.Tasks.Task<Result<IEnumerable<TaskDto>>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await taskRepository.GetUserTasksAsync(request.UserId);
            
            var taskDtos = tasks.Select(task => new TaskDto
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
            }).ToList();

            return Result<IEnumerable<TaskDto>>.Success(taskDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TaskDto>>.Failure(ex.Message);
        }
    }
} 