using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.ProjectMembers.Commands.JoinProject;

public class JoinProjectCommand : IRequest<Result<string>>
{
    public string ProjectId { get; set; } = string.Empty;
    public ProjectRole Role { get; set; }
} 