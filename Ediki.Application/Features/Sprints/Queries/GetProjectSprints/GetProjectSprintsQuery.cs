using Ediki.Application.Features.Sprints.DTOs;
using MediatR;

namespace Ediki.Application.Features.Sprints.Queries.GetProjectSprints;

public class GetProjectSprintsQuery : IRequest<IEnumerable<SprintDto>>
{
    public string ProjectId { get; set; } = string.Empty;
} 