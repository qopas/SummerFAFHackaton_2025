using Ediki.Application.Features.Projects.DTOs;
using MediatR;

namespace Ediki.Application.Features.Projects.Commands.ToggleProjectFeature;

public record ToggleProjectFeatureCommand(string ProjectId) : IRequest<ProjectDto>; 