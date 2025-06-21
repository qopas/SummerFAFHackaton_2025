using Ediki.Api.DTOs.Out.Tasks;
using Ediki.Application.Features.Tasks.Commands.AssignTask;
using Ediki.Application.Features.Tasks.Commands.CreateTask;
using Ediki.Application.Features.Tasks.Commands.DeleteTask;
using Ediki.Application.Features.Tasks.Commands.UpdateTask;
using Ediki.Application.Features.Tasks.Commands.UpdateTaskStatus;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Application.Features.Tasks.Queries.GetTaskById;
using Ediki.Application.Features.Tasks.Queries.GetUserTasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummerFAFHackaton_2025.Controllers;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class TasksController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTaskById(string id)
    {
        var query = new GetTaskByIdQuery { Id = id };
        return await ExecuteQueryAsync<TaskResponse, TaskDto?>(query);
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTasks(string userId)
    {
        var query = new GetUserTasksQuery { UserId = userId };
        return await ExecuteListQueryAsync<TaskResponse, TaskDto>(query);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetTaskById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskCommand command)
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

    [HttpPatch("{id}/assign")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignTask(string id, [FromBody] AssignTaskCommand command)
    {
        if (id != command.TaskId)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaskStatus(string id, [FromBody] UpdateTaskStatusCommand command)
    {
        if (id != command.TaskId)
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
    public async Task<IActionResult> DeleteTask(string id)
    {
        var command = new DeleteTaskCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error, errors = result.Errors });
        }
        
        if (!result.Value)
            return NotFound($"Task with ID {id} not found.");
            
        return NoContent();
    }
} 