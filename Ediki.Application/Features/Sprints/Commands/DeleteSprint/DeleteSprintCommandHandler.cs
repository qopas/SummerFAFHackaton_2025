using Ediki.Application.Common.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.DeleteSprint;

public class DeleteSprintCommandHandler(ISprintRepository sprintRepository) : IRequestHandler<DeleteSprintCommand, bool>
{
    public async System.Threading.Tasks.Task<bool> Handle(DeleteSprintCommand request, CancellationToken cancellationToken)
    {
        return await sprintRepository.DeleteAsync(request.Id);
    }
} 