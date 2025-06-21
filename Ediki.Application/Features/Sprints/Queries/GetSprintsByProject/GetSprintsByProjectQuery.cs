using Ediki.Application.Features.Sprints.DTOs;
using MediatR;

namespace Ediki.Application.Features.Sprints.Queries.GetSprintsByProject;

public class GetSprintsByProjectQuery : IRequest<IEnumerable<SprintDto>>
{
    public string ProjectId { get; set; } = string.Empty;
} 