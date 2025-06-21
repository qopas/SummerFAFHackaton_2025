using Ediki.Domain.Enums;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Domain.Entities;

public class Task
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SprintId { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AssigneeId { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public float? EstimatedHours { get; set; }
    public float? ActualHours { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Dependencies { get; set; } = new();
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Sprint Sprint { get; set; } = null!;
    public Project Project { get; set; } = null!;
    public ApplicationUser? Assignee { get; set; }
    public ApplicationUser CreatedByUser { get; set; } = null!;
} 