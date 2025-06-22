using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommand : IRequest<Result<TaskDto>>
{
    public string? SprintId { get; set; }
    public string ProjectId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AssigneeId { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public float? EstimatedHours { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Dependencies { get; set; } = new();
    public DateTime? DueDate { get; set; }
} 