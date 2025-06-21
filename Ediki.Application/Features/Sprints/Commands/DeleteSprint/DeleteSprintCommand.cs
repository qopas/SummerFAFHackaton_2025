using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.DeleteSprint;

public class DeleteSprintCommand : IRequest<Result<bool>>
{
    public string Id { get; set; } = string.Empty;
} 