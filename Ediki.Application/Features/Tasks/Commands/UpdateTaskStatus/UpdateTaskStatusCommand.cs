using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommand : IRequest<Result<TaskDto>>
{
    public string TaskId { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
} 