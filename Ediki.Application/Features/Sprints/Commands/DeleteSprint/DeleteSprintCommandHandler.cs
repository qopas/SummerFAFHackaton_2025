using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.DeleteSprint;

public class DeleteSprintCommandHandler(ISprintRepository sprintRepository) : IRequestHandler<DeleteSprintCommand, Result<bool>>
{
    public async System.Threading.Tasks.Task<Result<bool>> Handle(DeleteSprintCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sprintRepository.DeleteAsync(request.Id);
            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }
} 