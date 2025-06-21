using Ediki.Api.DTOs.Out.Sprints;
using Ediki.Application.Features.Sprints.Commands.CreateSprint;
using Ediki.Application.Features.Sprints.Commands.DeleteSprint;
using Ediki.Application.Features.Sprints.Commands.UpdateSprint;
using Ediki.Application.Features.Sprints.Commands.UpdateSprintStatus;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Application.Features.Sprints.Queries.GetProjectSprints;
using Ediki.Application.Features.Sprints.Queries.GetSprintById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummerFAFHackaton_2025.Controllers;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class SprintsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SprintDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSprintById(string id)
    {
        var query = new GetSprintByIdQuery { Id = id };
        return await ExecuteQueryAsync<SprintResponse, SprintDto?>(query);
    }

    [HttpGet("project/{projectId}")]
    [ProducesResponseType(typeof(IEnumerable<SprintDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectSprints(string projectId)
    {
        var query = new GetProjectSprintsQuery { ProjectId = projectId };
        return await ExecuteListQueryAsync<SprintResponse, SprintDto>(query);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SprintDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSprint([FromBody] CreateSprintCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetSprintById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SprintDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSprint(string id, [FromBody] UpdateSprintCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(SprintDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSprintStatus(string id, [FromBody] UpdateSprintStatusCommand command)
    {
        if (id != command.SprintId)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSprint(string id)
    {
        var command = new DeleteSprintCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }
        
        if (!result.Value)
            return NotFound($"Sprint with ID {id} not found.");
            
        return NoContent();
    }
} 