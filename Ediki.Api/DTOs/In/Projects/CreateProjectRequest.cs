using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Projects.Commands.CreateProject;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace SummerFAFHackaton_2025.DTOs.In.Projects;

public class CreateProjectRequest : IRequestIn<CreateProjectCommand>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public List<string> RolesNeeded { get; set; } = new();
    public int MaxParticipants { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public int DurationInWeeks { get; set; }
    public DateTime Deadline { get; set; }
    public string Company { get; set; } = string.Empty;
    public string? CompanyLogo { get; set; }
    public float? Budget { get; set; }
    public ProjectRewards? Rewards { get; set; }
    public string? Requirements { get; set; }
    public List<string>? Deliverables { get; set; }
    public List<ProjectResource>? Resources { get; set; }
    public bool IsPublic { get; set; }

    public CreateProjectRequest() { }

    public CreateProjectRequest(
        string title,
        string description,
        string shortDescription,
        List<string> tags,
        List<string> rolesNeeded,
        int maxParticipants,
        string difficulty,
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

    public CreateProjectCommand Convert()
    {
        if (!Enum.TryParse<Difficulty>(Difficulty, true, out var difficulty))
        {
            throw new ArgumentException($"Invalid difficulty value: {Difficulty}. Valid values are: {string.Join(", ", Enum.GetNames<Difficulty>())}");
        }

        var rolesNeeded = RolesNeeded.Select(role =>
        {
            if (!Enum.TryParse<ProjectRole>(role, true, out var projectRole))
            {
                throw new ArgumentException($"Invalid role value: {role}. Valid values are: {string.Join(", ", Enum.GetNames<ProjectRole>())}");
            }
            return projectRole;
        }).ToList();

        return new CreateProjectCommand(
            Title,
            Description,
            ShortDescription,
            Tags,
            rolesNeeded,
            MaxParticipants,
            difficulty,
            Duration,
            DurationInWeeks,
            Deadline,
            Company,
            CompanyLogo,
            Budget,
            Rewards,
            Requirements,
            Deliverables,
            Resources,
            IsPublic
        );
    }
} 