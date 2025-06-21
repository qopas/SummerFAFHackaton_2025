using Ediki.Application.Features.Tasks.DTOs;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetUserTasks;

public class GetUserTasksQuery : IRequest<IEnumerable<TaskDto>>
{
    public string UserId { get; set; } = string.Empty;
} 