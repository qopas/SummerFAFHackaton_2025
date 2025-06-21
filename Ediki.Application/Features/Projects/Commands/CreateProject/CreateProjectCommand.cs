using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<Result<ProjectDto>>
{
    public string Title { get; }
    public string Description { get; }
    public string ShortDescription { get; }
    public List<string> Tags { get; }
    public List<ProjectRole> RolesNeeded { get; }
    public int MaxParticipants { get; }
    public Difficulty Difficulty { get; }
    public string Duration { get; }
    public int DurationInWeeks { get; }
    public DateTime Deadline { get; }
    public string Company { get; }
    public string? CompanyLogo { get; }
    public float? Budget { get; }
    public ProjectRewards? Rewards { get; }
    public string? Requirements { get; }
    public List<string>? Deliverables { get; }
    public List<ProjectResource>? Resources { get; }
    public bool IsPublic { get; }

    public CreateProjectCommand(
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
        string? companyLogo = null,
        float? budget = null,
        ProjectRewards? rewards = null,
        string? requirements = null,
        List<string>? deliverables = null,
        List<ProjectResource>? resources = null,
        bool isPublic = true)
    {
        Title = title;
        Description = description;
        ShortDescription = shortDescription;
        Tags = tags;
        RolesNeeded = rolesNeeded;
        MaxParticipants = maxParticipants;
        Difficulty = difficulty;
        Duration = duration;
        DurationInWeeks = durationInWeeks;
        Deadline = deadline;
        Company = company;
        CompanyLogo = companyLogo;
        Budget = budget;
        Rewards = rewards;
        Requirements = requirements;
        Deliverables = deliverables;
        Resources = resources;
        IsPublic = isPublic;
    }
} 