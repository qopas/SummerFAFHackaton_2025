using Ediki.Application.Features.Tasks.DTOs;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQuery : IRequest<TaskDto?>
{
    public string Id { get; set; } = string.Empty;
} 