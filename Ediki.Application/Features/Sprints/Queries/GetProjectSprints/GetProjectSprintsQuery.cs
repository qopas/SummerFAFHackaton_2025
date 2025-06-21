using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Sprints.Queries.GetProjectSprints;

public class GetProjectSprintsQuery : IRequest<Result<IEnumerable<SprintDto>>>
{
    public string ProjectId { get; set; } = string.Empty;
} 