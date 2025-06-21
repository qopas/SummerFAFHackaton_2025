using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Sprints.Commands.UpdateSprintStatus;

public class UpdateSprintStatusCommand : IRequest<SprintDto>
{
    public string SprintId { get; set; } = string.Empty;
    public SprintStatus Status { get; set; }
} 