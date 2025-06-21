using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.UpdateProjectVisibility;

public class UpdateProjectVisibilityCommand : IRequest<Result<ProjectDto>>
{
    public string ProjectId { get; }
    public bool IsPublic { get; }

    public UpdateProjectVisibilityCommand(string projectId, bool isPublic)
    {
        ProjectId = projectId;
        IsPublic = isPublic;
    }
} 