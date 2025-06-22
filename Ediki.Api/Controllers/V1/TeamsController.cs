using Ediki.Api.DTOs.In;
using Ediki.Api.DTOs.Out.Teams;
using Ediki.Application.Features.Teams.Commands.CreateTeam;
using Ediki.Application.Features.Teams.Commands.InviteUser;
using Ediki.Application.Features.Teams.Commands.JoinTeam;
using Ediki.Application.Features.Teams.Commands.RemoveTeamMember;
using Ediki.Application.Features.Teams.Commands.UpdateMemberRole;
using Ediki.Application.Features.Teams.Commands.UpdateTeam;
using Ediki.Application.Features.Teams.DTOs;
using Ediki.Application.Features.Teams.Queries.GetTeamById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SummerFAFHackaton_2025.DTOs.In.Teams;
using SummerFAFHackaton_2025.DTOs.Out.Teams;

namespace SummerFAFHackaton_2025.Controllers.V1;

[Route("api/v1/[controller]")]
public class TeamsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpPost("projects/{projectId}")]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateTeam(string projectId, [FromBody] CreateTeamRequest request)
    {
        var requestWrapper = new CreateTeamRequestWrapper(request, projectId);
        return await ExecuteAsync<CreateTeamCommand, TeamResponse, TeamDto>(requestWrapper);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeamById(string id)
    {
        return await ExecuteQueryAsync<TeamResponse, TeamDto>(new GetTeamByIdQuery(id));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTeam(string id, [FromBody] UpdateTeamRequest request)
    {
        var requestWrapper = new UpdateTeamRequestWrapper(request, id);
        return await ExecuteAsync<UpdateTeamCommand, TeamResponse, TeamDto>(requestWrapper);
    }

    [HttpPost("{id}/invite")]
    [ProducesResponseType(typeof(TeamMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InviteUser(string id, [FromBody] InviteUserRequest request)
    {
        var requestWrapper = new InviteUserRequestWrapper(request, id);
        return await ExecuteAsync<InviteUserCommand, TeamMemberResponse, TeamMemberDto>(requestWrapper);
    }

    [HttpPost("{id}/join")]
    [ProducesResponseType(typeof(TeamMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> JoinTeam(string id, [FromBody] JoinTeamRequest request)
    {
        var requestWrapper = new JoinTeamRequestWrapper(request, id);
        return await ExecuteAsync<JoinTeamCommand, TeamMemberResponse, TeamMemberDto>(requestWrapper);
    }

    [HttpDelete("{id}/members/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveTeamMember(string id, string userId)
    {
        var requestWrapper = new RemoveTeamMemberRequestWrapper(id, userId);
        return await ExecuteAsync<RemoveTeamMemberCommand, BooleanResponse, bool>(requestWrapper);
    }

    [HttpPut("{id}/members/{userId}")]
    [ProducesResponseType(typeof(TeamMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMemberRole(string id, string userId, [FromBody] UpdateMemberRoleRequest request)
    {
        var requestWrapper = new UpdateMemberRoleRequestWrapper(request, id, userId);
        return await ExecuteAsync<UpdateMemberRoleCommand, TeamMemberResponse, TeamMemberDto>(requestWrapper);
    }
}

public class UpdateTeamRequestWrapper : IRequestIn<UpdateTeamCommand>
{
    private readonly UpdateTeamRequest _request;
    private readonly string _teamId;

    public UpdateTeamRequestWrapper(UpdateTeamRequest request, string teamId)
    {
        _request = request;
        _teamId = teamId;
    }

    public UpdateTeamCommand Convert()
    {
        return _request.Convert(_teamId);
    }
}

public class InviteUserRequestWrapper : IRequestIn<InviteUserCommand>
{
    private readonly InviteUserRequest _request;
    private readonly string _teamId;

    public InviteUserRequestWrapper(InviteUserRequest request, string teamId)
    {
        _request = request;
        _teamId = teamId;
    }

    public InviteUserCommand Convert()
    {
        return _request.Convert(_teamId);
    }
}

public class JoinTeamRequestWrapper : IRequestIn<JoinTeamCommand>
{
    private readonly JoinTeamRequest _request;
    private readonly string _teamId;

    public JoinTeamRequestWrapper(JoinTeamRequest request, string teamId)
    {
        _request = request;
        _teamId = teamId;
    }

    public JoinTeamCommand Convert()
    {
        return _request.Convert(_teamId);
    }
}

public class RemoveTeamMemberRequestWrapper : IRequestIn<RemoveTeamMemberCommand>
{
    private readonly string _teamId;
    private readonly string _userId;

    public RemoveTeamMemberRequestWrapper(string teamId, string userId)
    {
        _teamId = teamId;
        _userId = userId;
    }

    public RemoveTeamMemberCommand Convert()
    {
        return new RemoveTeamMemberCommand(_teamId, _userId);
    }
}

public class UpdateMemberRoleRequestWrapper : IRequestIn<UpdateMemberRoleCommand>
{
    private readonly UpdateMemberRoleRequest _request;
    private readonly string _teamId;
    private readonly string _userId;

    public UpdateMemberRoleRequestWrapper(UpdateMemberRoleRequest request, string teamId, string userId)
    {
        _request = request;
        _teamId = teamId;
        _userId = userId;
    }

    public UpdateMemberRoleCommand Convert()
    {
        return _request.Convert(_teamId, _userId);
    }
}

public class CreateTeamRequestWrapper : IRequestIn<CreateTeamCommand>
{
    private readonly CreateTeamRequest _request;
    private readonly string _projectId;

    public CreateTeamRequestWrapper(CreateTeamRequest request, string projectId)
    {
        _request = request;
        _projectId = projectId;
    }

    public CreateTeamCommand Convert()
    {
        return _request.Convert(_projectId);
    }
}