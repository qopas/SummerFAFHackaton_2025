using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace Ediki.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ShortDescription { get; private set; } = string.Empty;
    public List<string> Tags { get; private set; } = new();
    public List<ProjectRole> RolesNeeded { get; private set; } = new();
    public int MaxParticipants { get; private set; }
    public int CurrentParticipants { get; private set; }
    public Difficulty Difficulty { get; private set; }
    public string Duration { get; private set; } = string.Empty;
    public int DurationInWeeks { get; private set; }
    public DateTime Deadline { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Company { get; private set; } = string.Empty;
    public string? CompanyLogo { get; private set; }
    public ProjectStatus Status { get; set; }
    public float Progress { get; private set; }
    public float? Budget { get; private set; }
    public ProjectRewards? Rewards { get; private set; }
    public string? Requirements { get; private set; }
    public List<string>? Deliverables { get; private set; }
    public List<ProjectResource>? Resources { get; private set; }
    public bool IsPublic { get; set; }
    public bool IsFeatured { get; private set; }
    public string CreatedById { get; private set; } = string.Empty;

    private Project() { }

    public static Project Create(
        string title,
        string description,
        string shortDescription,
        List<string> tags,
        List<ProjectRole> rolesNeeded,
        int maxParticipants,
        Difficulty difficulty,
        string duration,
        int durationInWeeks,
        DateTime deadline,
        string company,
        string createdById,
        string? companyLogo = null,
        float? budget = null,
        ProjectRewards? rewards = null,
        string? requirements = null,
        List<string>? deliverables = null,
        List<ProjectResource>? resources = null,
        bool isPublic = true)
    {
        var project = new Project
        {
            Title = title,
            Description = description,
            ShortDescription = shortDescription,
            Tags = tags,
            RolesNeeded = rolesNeeded,
            MaxParticipants = maxParticipants,
            CurrentParticipants = 0,
            Difficulty = difficulty,
            Duration = duration,
            DurationInWeeks = durationInWeeks,
            Deadline = deadline,
            Company = company,
            CompanyLogo = companyLogo,
            Status = ProjectStatus.Draft,
            Progress = 0,
            Budget = budget,
            Rewards = rewards,
            Requirements = requirements,
            Deliverables = deliverables,
            Resources = resources,
            IsPublic = isPublic,
            IsFeatured = false,
            CreatedById = createdById
        };

        return project;
    }

    public void UpdateStatus(ProjectStatus status)
    {
        Status = status;
    }

    public void UpdateProgress(float progress)
    {
        if (progress < 0 || progress > 100)
            throw new ArgumentException("Progress must be between 0 and 100");

        Progress = progress;
    }

    public void SetFeatured(bool isFeatured)
    {
        IsFeatured = isFeatured;
    }

    public void SetPublic(bool isPublic)
    {
        IsPublic = isPublic;
    }

    public void UpdateCompanyLogo(string? logo)
    {
        CompanyLogo = logo;
    }

    public void UpdateBudget(float? budget)
    {
        Budget = budget;
    }

    public void UpdateRewards(ProjectRewards rewards)
    {
        Rewards = rewards;
    }

    public void UpdateRequirements(string requirements)
    {
        Requirements = requirements;
    }

    public void UpdateDeliverables(List<string> deliverables)
    {
        Deliverables = deliverables;
    }

    public void AddResource(ProjectResource resource)
    {
        Resources ??= new List<ProjectResource>();
        Resources.Add(resource);
    }

    public void Start()
    {
        if (Status != ProjectStatus.Draft)
            throw new InvalidOperationException("Project must be in Draft status to start");

        Status = ProjectStatus.InProgress;
        StartDate = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != ProjectStatus.InProgress)
            throw new InvalidOperationException("Project must be in InProgress status to complete");

        Status = ProjectStatus.Completed;
        EndDate = DateTime.UtcNow;
        Progress = 100;
    }

    public void Cancel()
    {
        if (Status == ProjectStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed project");

        Status = ProjectStatus.Cancelled;
        EndDate = DateTime.UtcNow;
    }
} 