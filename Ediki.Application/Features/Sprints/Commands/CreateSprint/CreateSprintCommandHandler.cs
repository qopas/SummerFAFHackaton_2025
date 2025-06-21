using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.CreateSprint;

public class CreateSprintCommandHandler(ISprintRepository sprintRepository) : IRequestHandler<CreateSprintCommand, Result<SprintDto>>
{
    public async System.Threading.Tasks.Task<Result<SprintDto>> Handle(CreateSprintCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sprint = new Sprint
            {
                ProjectId = request.ProjectId,
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                Order = request.Order,
                Goals = request.Goals,
                Deliverables = request.Deliverables
            };

            var createdSprint = await sprintRepository.CreateAsync(sprint);

            var sprintDto = new SprintDto
            {
                Id = createdSprint.Id,
                ProjectId = createdSprint.ProjectId,
                Name = createdSprint.Name,
                Description = createdSprint.Description,
                StartDate = createdSprint.StartDate,
                EndDate = createdSprint.EndDate,
                Status = createdSprint.Status,
                Order = createdSprint.Order,
                Goals = createdSprint.Goals,
                Deliverables = createdSprint.Deliverables,
                CreatedAt = createdSprint.CreatedAt,
                UpdatedAt = createdSprint.UpdatedAt,
                TaskCount = 0,
                CompletedTaskCount = 0
            };

            return Result<SprintDto>.Success(sprintDto);
        }
        catch (Exception ex)
        {
            return Result<SprintDto>.Failure(ex.Message);
        }
    }
} 