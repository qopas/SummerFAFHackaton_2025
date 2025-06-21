using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.DeleteSprint;

public class DeleteSprintCommand : IRequest<bool>
{
    public string Id { get; set; } = string.Empty;
} 