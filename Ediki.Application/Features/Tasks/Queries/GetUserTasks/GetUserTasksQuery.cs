using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Queries.GetUserTasks;

public class GetUserTasksQuery : IRequest<Result<IEnumerable<TaskDto>>>
{
    public string UserId { get; set; } = string.Empty;
} 