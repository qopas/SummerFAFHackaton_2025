using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.UpdateSprintStatus;

public class UpdateSprintStatusCommandHandler(ISprintRepository sprintRepository) : IRequestHandler<UpdateSprintStatusCommand, Result<SprintDto>>
{
    public async System.Threading.Tasks.Task<Result<SprintDto>> Handle(UpdateSprintStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sprint = await sprintRepository.GetByIdAsync(request.SprintId);
            if (sprint == null)
                return Result<SprintDto>.Failure($"Sprint with ID {request.SprintId} not found.");

            sprint.Status = request.Status;
            var updatedSprint = await sprintRepository.UpdateAsync(sprint);

            var sprintDto = new SprintDto
            {
                Id = updatedSprint.Id,
                ProjectId = updatedSprint.ProjectId,
                Name = updatedSprint.Name,
                Description = updatedSprint.Description,
                StartDate = updatedSprint.StartDate,
                EndDate = updatedSprint.EndDate,
                Status = updatedSprint.Status,
                Order = updatedSprint.Order,
                Goals = updatedSprint.Goals,
                Deliverables = updatedSprint.Deliverables,
                CreatedAt = updatedSprint.CreatedAt,
                UpdatedAt = updatedSprint.UpdatedAt,
                TaskCount = updatedSprint.Tasks?.Count ?? 0,
                CompletedTaskCount = updatedSprint.Tasks?.Count(t => t.Status == Ediki.Domain.Enums.TaskStatus.Completed) ?? 0
            };

            return Result<SprintDto>.Success(sprintDto);
        }
        catch (Exception ex)
        {
            return Result<SprintDto>.Failure(ex.Message);
        }
    }
} 