using Ediki.Api.DTOs.Out;
using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace SummerFAFHackaton_2025.DTOs.Out.Projects;

public class ProjectResponse : IResponseOut<ProjectDto>
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public List<ProjectRole> RolesNeeded { get; set; } = new();
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public Difficulty Difficulty { get; set; }
    public string Duration { get; set; } = string.Empty;
    public int DurationInWeeks { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Company { get; set; } = string.Empty;
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
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ProjectResponse() { }

    public ProjectResponse(ProjectDto result)
    {
        Id = result.Id;
        Title = result.Title;
        Description = result.Description;
        ShortDescription = result.ShortDescription;
        Tags = result.Tags;
        RolesNeeded = result.RolesNeeded;
        MaxParticipants = result.MaxParticipants;
        CurrentParticipants = result.CurrentParticipants;
        Difficulty = result.Difficulty;
        Duration = result.Duration;
        DurationInWeeks = result.DurationInWeeks;
        Deadline = result.Deadline;
        StartDate = result.StartDate;
        EndDate = result.EndDate;
        Company = result.Company;
        CompanyLogo = result.CompanyLogo;
        Status = result.Status;
        Progress = result.Progress;
        Budget = result.Budget;
        Rewards = result.Rewards;
        Requirements = result.Requirements;
        Deliverables = result.Deliverables;
        Resources = result.Resources;
        IsPublic = result.IsPublic;
        IsFeatured = result.IsFeatured;
        CreatedById = result.CreatedById;
        CreatedAt = result.CreatedAt;
        UpdatedAt = result.UpdatedAt;
    }

    public object? Convert(ProjectDto result)
    {
        return new ProjectResponse(result);
    }
} 