using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Projects.Queries.GetProjects;
using Ediki.Domain.Enums;

namespace SummerFAFHackaton_2025.DTOs.In.Projects;

public class GetProjectsRequest : IRequestIn<GetProjectsQuery>
{
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public string? Difficulty { get; set; }
    public string? Role { get; set; }
    public bool? IsFeatured { get; set; }
    public bool? IncludePrivate { get; set; }
    public string? UserId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public GetProjectsRequest() { }

    public GetProjectsRequest(
        string? searchTerm,
        string? status,
        string? difficulty,
        string? role,
        bool? isFeatured,
        bool? includePrivate,
        string? userId,
        int page = 1,
        int pageSize = 10)
    {
        SearchTerm = searchTerm;
        Status = status;
        Difficulty = difficulty;
        Role = role;
        IsFeatured = isFeatured;
        IncludePrivate = includePrivate;
        UserId = userId;
        Page = page;
        PageSize = pageSize;
    }

    public GetProjectsQuery Convert()
    {
        ProjectStatus? status = null;
        Difficulty? difficulty = null;
        ProjectRole? role = null;

        if (!string.IsNullOrEmpty(Status) && Enum.TryParse<ProjectStatus>(Status, true, out var parsedStatus))
        {
            status = parsedStatus;
        }

        if (!string.IsNullOrEmpty(Difficulty) && Enum.TryParse<Difficulty>(Difficulty, true, out var parsedDifficulty))
        {
            difficulty = parsedDifficulty;
        }

        if (!string.IsNullOrEmpty(Role) && Enum.TryParse<ProjectRole>(Role, true, out var parsedRole))
        {
            role = parsedRole;
        }

        return new GetProjectsQuery(
            SearchTerm,
            status,
            difficulty,
            role,
            IsFeatured,
            IncludePrivate,
            UserId,
            Page,
            PageSize
        );
    }
} 