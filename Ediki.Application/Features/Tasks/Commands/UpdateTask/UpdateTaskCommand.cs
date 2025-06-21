using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<Result<TaskDto>>
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AssigneeId { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public float? EstimatedHours { get; set; }
    public float? ActualHours { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Dependencies { get; set; } = new();
    public DateTime? DueDate { get; set; }
} 