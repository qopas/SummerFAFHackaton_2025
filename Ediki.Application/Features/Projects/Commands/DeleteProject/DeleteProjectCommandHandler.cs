using Ediki.Application.Exceptions;
using Ediki.Application.Features.Projects.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteProjectCommandHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ProjectDto>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
            throw new NotFoundException("Project", request.ProjectId);

        var isOwner = await _projectRepository.IsOwnerAsync(request.ProjectId, userId);
        if (!isOwner)
            throw new UnauthorizedAccessException("Only the project creator can delete the project");

        await _projectRepository.DeleteAsync(project);

        // Convert to DTO before returning
        var projectDto = new ProjectDto
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
        };

        return Result<ProjectDto>.Success(projectDto);
    }
} 