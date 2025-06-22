using Ediki.Application.Common.Interfaces;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using MediatR;

namespace Ediki.Application.Features.ProjectMembers.Commands.JoinProject;

public class JoinProjectCommandHandler(
    IProjectMemberRepository projectMemberRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<JoinProjectCommand, Result<string>>
{
    public async Task<Result<string>> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

        var canJoin = await projectMemberRepository.CanUserJoinProjectAsync(request.ProjectId, userId, request.Role);
        if (!canJoin)
        {
            return Result<string>.Failure("Cannot join project. Either you're already a member, the project doesn't need this role, or the project is full.");
        }

        var projectMember = ProjectMember.Create(
            projectId: request.ProjectId,
            userId: userId,
            role: request.Role);

        await projectMemberRepository.AddAsync(projectMember);

        return Result<string>.Success(projectMember.Id);
    }
} 