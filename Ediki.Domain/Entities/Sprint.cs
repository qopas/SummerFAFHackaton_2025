using Ediki.Domain.Enums;

namespace Ediki.Domain.Entities;

public class Sprint
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ProjectId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SprintStatus Status { get; set; } = SprintStatus.Planned;
    public int Order { get; set; }
    public List<string> Goals { get; set; } = new();
    public List<string> Deliverables { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Project Project { get; set; } = null!;
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
} 