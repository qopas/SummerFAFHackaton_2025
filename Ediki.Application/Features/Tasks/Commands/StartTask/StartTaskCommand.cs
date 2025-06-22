using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.StartTask;

public class StartTaskCommand : IRequest<Result<TaskDto>>
{
    public string TaskId { get; set; } = string.Empty;
} 