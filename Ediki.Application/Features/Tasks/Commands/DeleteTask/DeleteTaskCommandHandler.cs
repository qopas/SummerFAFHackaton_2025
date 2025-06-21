using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<DeleteTaskCommand, Result<bool>>
{
    public async System.Threading.Tasks.Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await taskRepository.ExistsAsync(request.Id);
            if (!exists)
                return Result<bool>.Success(false);

            var result = await taskRepository.DeleteAsync(request.Id);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }
} 