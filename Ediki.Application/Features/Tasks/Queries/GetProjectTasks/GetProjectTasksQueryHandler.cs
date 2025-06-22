using AutoMapper;
using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetProjectTasks;

public class GetProjectTasksQueryHandler(
    ITaskRepository taskRepository,
    IMapper mapper)
    : IRequestHandler<GetProjectTasksQuery, Result<IEnumerable<TaskDto>>>
{
    public async Task<Result<IEnumerable<TaskDto>>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await taskRepository.GetTasksWithFiltersAsync(
            projectId: request.ProjectId,
            sprintId: request.SprintId,
            status: request.Status,
            priority: request.Priority,
            assigneeId: request.AssigneeId);

        var taskDtos = mapper.Map<IEnumerable<TaskDto>>(tasks);
        return Result<IEnumerable<TaskDto>>.Success(taskDtos);
    }
} 