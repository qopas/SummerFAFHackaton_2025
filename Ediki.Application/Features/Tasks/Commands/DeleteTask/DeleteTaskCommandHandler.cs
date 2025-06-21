using Ediki.Application.Common.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var exists = await _taskRepository.ExistsAsync(request.Id);
        if (!exists)
            return false;

        return await _taskRepository.DeleteAsync(request.Id);
    }
} 