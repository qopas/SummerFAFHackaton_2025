using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Common;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Sprints.Commands.UpdateSprint;

public class UpdateSprintCommandHandler(ISprintRepository sprintRepository)
    : IRequestHandler<UpdateSprintCommand, Result<SprintDto>>
{
    public async System.Threading.Tasks.Task<Result<SprintDto>> Handle(UpdateSprintCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sprint = await sprintRepository.GetByIdAsync(request.Id);
            if (sprint == null)
                return Result<SprintDto>.Failure($"Sprint with ID {request.Id} not found.");

            sprint.Name = request.Name;
            sprint.Description = request.Description;
            sprint.StartDate = request.StartDate;
            sprint.EndDate = request.EndDate;
            sprint.Status = request.Status;
            sprint.Order = request.Order;
            sprint.Goals = request.Goals;
            sprint.Deliverables = request.Deliverables;

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
                CompletedTaskCount = updatedSprint.Tasks?.Count(t => t.Status == TaskStatus.Completed) ?? 0
            };

            return Result<SprintDto>.Success(sprintDto);
        }
        catch (Exception ex)
        {
            return Result<SprintDto>.Failure(ex.Message);
        }
    }
} 