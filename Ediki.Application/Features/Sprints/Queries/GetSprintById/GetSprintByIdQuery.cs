using Ediki.Application.Features.Sprints.DTOs;
using MediatR;

namespace Ediki.Application.Features.Sprints.Queries.GetSprintById;

public class GetSprintByIdQuery : IRequest<SprintDto?>
{
    public string Id { get; set; } = string.Empty;
} 