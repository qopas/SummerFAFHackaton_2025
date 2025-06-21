using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Projects.Queries.GetProjects;

public class GetProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
{
    public string? SearchTerm { get; }
    public ProjectStatus? Status { get; }
    public Difficulty? Difficulty { get; }
    public ProjectRole? Role { get; }
    public bool? IsFeatured { get; }
    public bool? IncludePrivate { get; }
    public string? UserId { get; }
    public int Page { get; }
    public int PageSize { get; }

    public GetProjectsQuery(
        string? searchTerm,
        ProjectStatus? status,
        Difficulty? difficulty,
        ProjectRole? role,
        bool? isFeatured,
        bool? includePrivate,
        string? userId,
        int page,
        int pageSize)
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
} 