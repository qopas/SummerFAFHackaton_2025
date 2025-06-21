using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Enums;
using MediatR;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Features.Sprints.Commands.UpdateSprint;

public class UpdateSprintCommandHandler(ISprintRepository sprintRepository)
    : IRequestHandler<UpdateSprintCommand, SprintDto>
{
    public async Task<SprintDto> Handle(UpdateSprintCommand request, CancellationToken cancellationToken)
    {
        var sprint = await sprintRepository.GetByIdAsync(request.Id);
        if (sprint == null)
            throw new KeyNotFoundException($"Sprint with ID {request.Id} not found.");

        sprint.Name = request.Name;
        sprint.Description = request.Description;
        sprint.StartDate = request.StartDate;
        sprint.EndDate = request.EndDate;
        sprint.Status = request.Status;
        sprint.Goals = request.Goals;
        sprint.Deliverables = request.Deliverables;

        var updatedSprint = await sprintRepository.UpdateAsync(sprint);

        var taskCount = updatedSprint.Tasks?.Count ?? 0;
        var completedTaskCount = updatedSprint.Tasks?.Count(t => t.Status == TaskStatus.Completed) ?? 0;

        return new SprintDto
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
            TaskCount = taskCount,
            CompletedTaskCount = completedTaskCount
        };
    }
} 