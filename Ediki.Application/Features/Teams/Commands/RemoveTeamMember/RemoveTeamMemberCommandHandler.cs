using Ediki.Domain.Common;
using Ediki.Domain.Interfaces;
using MediatR;

namespace Ediki.Application.Features.Teams.Commands.RemoveTeamMember;

public class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand, Result<bool>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public RemoveTeamMemberCommandHandler(
        ITeamRepository teamRepository,
        ITeamMemberRepository teamMemberRepository)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<Result<bool>> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
    {
        // Check if team exists
        var team = await _teamRepository.GetByIdAsync(request.TeamId);
        if (team == null)
        {
            return Result<bool>.Failure("Team not found");
        }

        // Check if team member exists
        var teamMember = await _teamMemberRepository.GetByTeamAndUserAsync(request.TeamId, request.UserId);
        if (teamMember == null)
        {
            return Result<bool>.Failure("Team member not found");
        }

        // Remove the team member
        await _teamMemberRepository.DeleteAsync(teamMember);

        return Result<bool>.Success(true);
    }
}