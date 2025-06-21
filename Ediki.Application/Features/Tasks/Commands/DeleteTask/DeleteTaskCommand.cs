using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest<bool>
{
    public string Id { get; set; } = string.Empty;
} 