using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Projects.Queries.GetProjectById;

public class GetProjectByIdQuery : IRequest<Result<ProjectDto>>
{
    public string ProjectId { get; }

    public GetProjectByIdQuery(string projectId)
    {
        ProjectId = projectId;
    }
} 