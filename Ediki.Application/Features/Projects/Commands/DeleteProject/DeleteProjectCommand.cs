using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectCommand(string projectId) : IRequest<Result<ProjectDto>>
{
    public string ProjectId { get; } = projectId;
} 