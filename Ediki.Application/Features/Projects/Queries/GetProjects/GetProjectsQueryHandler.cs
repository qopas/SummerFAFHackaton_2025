using Ediki.Application.Features.Projects.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler(
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetProjectsQuery, Result<IEnumerable<ProjectDto>>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(
            searchTerm: request.SearchTerm,
            status: request.Status,
            difficulty: request.Difficulty,
            role: request.Role,
            isFeatured: request.IsFeatured,
            includePrivate: request.IncludePrivate.Value,
            userId: _currentUserService.UserId,
            page: request.Page,
            pageSize: request.PageSize);

        var projectDtos = projects.Select(project => new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ShortDescription = project.ShortDescription,
            Tags = project.Tags,
            RolesNeeded = project.RolesNeeded,
            MaxParticipants = project.MaxParticipants,
            CurrentParticipants = project.CurrentParticipants,
            Difficulty = project.Difficulty,
            Duration = project.Duration,
            DurationInWeeks = project.DurationInWeeks,
            Deadline = project.Deadline,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Company = project.Company,
            CompanyLogo = project.CompanyLogo,
            Status = project.Status,
            Progress = project.Progress,
            Budget = project.Budget,
            Rewards = project.Rewards,
            Requirements = project.Requirements,
            Deliverables = project.Deliverables,
            Resources = project.Resources,
            IsPublic = project.IsPublic,
            IsFeatured = project.IsFeatured,
            CreatedById = project.CreatedById,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        });

        return Result<IEnumerable<ProjectDto>>.Success(projectDtos);
    }
} 