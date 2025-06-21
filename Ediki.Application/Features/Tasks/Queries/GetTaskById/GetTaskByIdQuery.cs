using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQuery : IRequest<Result<TaskDto?>>
{
    public string Id { get; set; } = string.Empty;
} 