using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Tasks.Queries.GetProjectTasks;

public class GetProjectTasksQuery : IRequest<Result<IEnumerable<TaskDto>>>
{
    public string ProjectId { get; set; } = string.Empty;
    public string? SprintId { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public string? AssigneeId { get; set; }
} 