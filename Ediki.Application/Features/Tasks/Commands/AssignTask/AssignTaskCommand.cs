using Ediki.Application.Features.Tasks.DTOs;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.AssignTask;

public class AssignTaskCommand : IRequest<TaskDto>
{
    public string TaskId { get; set; } = string.Empty;
    public string? AssigneeId { get; set; }
} 