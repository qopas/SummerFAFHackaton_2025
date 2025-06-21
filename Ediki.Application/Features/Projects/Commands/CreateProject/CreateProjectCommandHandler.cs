using Ediki.Application.Features.Projects.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using Ediki.Domain.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler(
    IProjectRepository projectRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
{
    public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

        var project = Project.Create(
            title: request.Title,
            description: request.Description,
            shortDescription: request.ShortDescription,
            tags: request.Tags,
            rolesNeeded: request.RolesNeeded,
            maxParticipants: request.MaxParticipants,
            difficulty: request.Difficulty,
            duration: request.Duration,
            durationInWeeks: request.DurationInWeeks,
            deadline: request.Deadline,
            company: request.Company,
            createdById: userId,
            companyLogo: request.CompanyLogo,
            budget: request.Budget,
            rewards: request.Rewards,
            requirements: request.Requirements,
            deliverables: request.Deliverables,
            resources: request.Resources,
            isPublic: request.IsPublic
        );

        await projectRepository.AddAsync(project);

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