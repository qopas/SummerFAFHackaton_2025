using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Sprints.Queries.GetSprintById;

public class GetSprintByIdQuery : IRequest<Result<SprintDto?>>
{
    public string Id { get; set; } = string.Empty;
} 