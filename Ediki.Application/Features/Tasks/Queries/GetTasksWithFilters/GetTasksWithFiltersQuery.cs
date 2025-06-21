using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Enums;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Tasks.Queries.GetTasksWithFilters;

public class GetTasksWithFiltersQuery : IRequest<IEnumerable<TaskDto>>
{
    public string? ProjectId { get; set; }
    public string? SprintId { get; set; }
    public string? AssigneeId { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
} 