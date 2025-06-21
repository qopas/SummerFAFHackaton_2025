using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest<Result<bool>>
{
    public string Id { get; set; } = string.Empty;
} 