using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace Ediki.Application.Features.Projects.DTOs;

public class ProjectDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public List<string> Tags { get; set; }
    public List<ProjectRole> RolesNeeded { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public Difficulty Difficulty { get; set; }
    public string Duration { get; set; }
    public int DurationInWeeks { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Company { get; set; }
    public string? CompanyLogo { get; set; }
    public ProjectStatus Status { get; set; }
    public float Progress { get; set; }
    public float? Budget { get; set; }
    public ProjectRewards? Rewards { get; set; }
    public string? Requirements { get; set; }
    public List<string>? Deliverables { get; set; }
    public List<ProjectResource>? Resources { get; set; }
    public bool IsPublic { get; set; }
    public bool IsFeatured { get; set; }
    public string CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 