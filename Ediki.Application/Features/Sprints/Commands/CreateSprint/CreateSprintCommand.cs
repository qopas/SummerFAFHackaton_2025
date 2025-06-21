using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.CreateSprint;

public class CreateSprintCommand : IRequest<SprintDto>
{
    public string ProjectId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SprintStatus Status { get; set; } = SprintStatus.Planned;
    public List<string> Goals { get; set; } = new();
    public List<string> Deliverables { get; set; } = new();
} 