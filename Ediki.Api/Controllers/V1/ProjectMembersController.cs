using Ediki.Api.DTOs.Out.ProjectMembers;
using Ediki.Application.Features.ProjectMembers.Commands.JoinProject;
using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Application.Features.ProjectMembers.Queries.GetProjectMembers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummerFAFHackaton_2025.Controllers;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/projects/{projectId}/members")]
public class ProjectMembersController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectMemberDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectMembers(
        string projectId, 
        [FromQuery] bool? isActive = null, 
        [FromQuery] bool? isProjectLead = null)
    {
        var query = new GetProjectMembersQuery 
        { 
            ProjectId = projectId,
            IsActive = isActive,
            IsProjectLead = isProjectLead
        };
        return await ExecuteListQueryAsync<ProjectMemberResponse, ProjectMemberDto>(query);
    }

    [HttpPost("join")]
    [ProducesResponseType(typeof(ProjectMemberDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinProject(string projectId, [FromBody] JoinProjectCommand command)
    {
        if (projectId != command.ProjectId)
            return BadRequest("Project ID mismatch");

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetProjectMembers), new { projectId = projectId }, result.Value);
    }
} 