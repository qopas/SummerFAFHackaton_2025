using Ediki.Application.Exceptions;
using Ediki.Application.Features.Projects.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.UpdateProjectProgress;

public class UpdateProjectProgressCommandHandler(
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateProjectProgressCommand, Result<ProjectDto>>
{
    public async Task<Result<ProjectDto>> Handle(UpdateProjectProgressCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
            throw new NotFoundException("Project", request.ProjectId);

        var isOwner = await projectRepository.IsOwnerAsync(request.ProjectId, userId);
        if (!isOwner)
            throw new UnauthorizedAccessException("Only the project creator can update the project progress");

        project.Status = request.Status;
        await projectRepository.UpdateAsync(project);

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