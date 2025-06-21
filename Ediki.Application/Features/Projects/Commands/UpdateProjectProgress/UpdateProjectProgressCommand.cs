using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.UpdateProjectProgress;

public class UpdateProjectProgressCommand : IRequest<Result<ProjectDto>>
{
    public string ProjectId { get; }
    public ProjectStatus Status { get; }

    public UpdateProjectProgressCommand(string projectId, ProjectStatus status)
    {
        ProjectId = projectId;
        Status = status;
    }
} 