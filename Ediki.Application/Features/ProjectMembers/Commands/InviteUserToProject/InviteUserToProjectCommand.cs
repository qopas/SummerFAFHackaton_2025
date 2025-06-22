using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.ProjectMembers.Commands.InviteUserToProject;

public class InviteUserToProjectCommand : IRequest<Result<ProjectMemberDto>>
{
    public string ProjectId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ProjectRole Role { get; set; }
} 