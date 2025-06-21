using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.AssignTask;

public class AssignTaskCommand : IRequest<Result<TaskDto>>
{
    public string TaskId { get; set; } = string.Empty;
    public string? AssigneeId { get; set; }
} 