using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Entities;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.CreateSprint;

public class CreateSprintCommandHandler : IRequestHandler<CreateSprintCommand, SprintDto>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateSprintCommandHandler(ISprintRepository sprintRepository, ICurrentUserService currentUserService)
    {
        _sprintRepository = sprintRepository;
        _currentUserService = currentUserService;
    }

    public async Task<SprintDto> Handle(CreateSprintCommand request, CancellationToken cancellationToken)
    {
        var nextOrder = await _sprintRepository.GetNextOrderAsync(request.ProjectId);
        
        var sprint = new Sprint
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            Order = nextOrder,
            Goals = request.Goals,
            Deliverables = request.Deliverables
        };

        var createdSprint = await _sprintRepository.CreateAsync(sprint);

        return new SprintDto
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
    }
} 