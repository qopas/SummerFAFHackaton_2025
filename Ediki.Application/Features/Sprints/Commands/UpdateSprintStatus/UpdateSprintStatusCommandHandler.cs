using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.UpdateSprintStatus;

public class UpdateSprintStatusCommandHandler(ISprintRepository sprintRepository) : IRequestHandler<UpdateSprintStatusCommand, SprintDto>
{
    public async System.Threading.Tasks.Task<SprintDto> Handle(UpdateSprintStatusCommand request, CancellationToken cancellationToken)
    {
        var sprint = await sprintRepository.GetByIdAsync(request.SprintId);
        if (sprint == null)
            throw new KeyNotFoundException($"Sprint with ID {request.SprintId} not found.");

        sprint.Status = request.Status;
        var updatedSprint = await sprintRepository.UpdateAsync(sprint);

        return new SprintDto
        {
            Id = updatedSprint.Id,
            ProjectId = updatedSprint.ProjectId,
            Name = updatedSprint.Name,
            Description = updatedSprint.Description,
            StartDate = updatedSprint.StartDate,
            EndDate = updatedSprint.EndDate,
            Status = updatedSprint.Status,
            Goals = updatedSprint.Goals,
            CreatedBy = updatedSprint.CreatedBy,
            CreatedByName = updatedSprint.CreatedByUser?.UserName ?? string.Empty,
            CreatedAt = updatedSprint.CreatedAt,
            UpdatedAt = updatedSprint.UpdatedAt
        };
    }
} 