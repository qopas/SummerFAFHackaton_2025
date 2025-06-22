using Ediki.Api.DTOs.Out.Tasks;
using Ediki.Application.Features.Tasks.Commands.AssignTask;
using Ediki.Application.Features.Tasks.Commands.CreateTask;
using Ediki.Application.Features.Tasks.Commands.DeleteTask;
using Ediki.Application.Features.Tasks.Commands.StartTask;
using Ediki.Application.Features.Tasks.Commands.SubmitForReview;
using Ediki.Application.Features.Tasks.Commands.UpdateTask;
using Ediki.Application.Features.Tasks.Commands.UpdateTaskStatus;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Application.Features.Tasks.Queries.GetTaskById;
using Ediki.Application.Features.Tasks.Queries.GetProjectTasks;
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

    [HttpGet("project/{projectId}")]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectTasks(
        string projectId, 
        [FromQuery] string? sprintId = null, 
        [FromQuery] string? status = null, 
        [FromQuery] string? priority = null, 
        [FromQuery] string? assigneeId = null)
    {
        Domain.Enums.TaskStatus? taskStatus = null;
        Domain.Enums.TaskPriority? taskPriority = null;

        if (!string.IsNullOrEmpty(status))
        {
            taskStatus = status.ToLower() switch
            {
                "todo" => Domain.Enums.TaskStatus.Todo,
                "in-progress" => Domain.Enums.TaskStatus.InProgress,
                "review" => Domain.Enums.TaskStatus.Review,
                "completed" => Domain.Enums.TaskStatus.Completed,
                _ => int.TryParse(status, out var statusInt) ? (Domain.Enums.TaskStatus)statusInt : 
                     Enum.TryParse<Domain.Enums.TaskStatus>(status, true, out var statusEnum) ? statusEnum : null
            };
        }

        if (!string.IsNullOrEmpty(priority))
        {
            taskPriority = priority.ToLower() switch
            {
                "low" => Domain.Enums.TaskPriority.Low,
                "medium" => Domain.Enums.TaskPriority.Medium,
                "high" => Domain.Enums.TaskPriority.High,
                "urgent" => Domain.Enums.TaskPriority.Urgent,
                _ => int.TryParse(priority, out var priorityInt) ? (Domain.Enums.TaskPriority)priorityInt :
                     Enum.TryParse<Domain.Enums.TaskPriority>(priority, true, out var priorityEnum) ? priorityEnum : null
            };
        }

        var query = new GetProjectTasksQuery 
        { 
            ProjectId = projectId,
            SprintId = sprintId,
            Status = taskStatus,
            Priority = taskPriority,
            AssigneeId = assigneeId
        };
        return await ExecuteListQueryAsync<TaskResponse, TaskDto>(query);
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
            return BadRequest(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }

        return CreatedAtAction(nameof(GetTaskById), new { id = result.Value.Id }, new {
            success = true,
            data = result.Value,
            message = "Task created successfully"
        });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskCommand command)
    {
        if (id != command.Id)
            return BadRequest(new {
                success = false,
                message = "ID mismatch",
                errors = new[] { "Task ID in URL does not match ID in request body" }
            });

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }

        return Ok(new {
            success = true,
            data = result.Value,
            message = "Task updated successfully"
        });
    }

    [HttpPatch("{id}/assign")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignTask(string id, [FromBody] AssignTaskCommand command)
    {
        if (id != command.TaskId)
            return BadRequest(new {
                success = false,
                message = "ID mismatch",
                errors = new[] { "Task ID in URL does not match ID in request body" }
            });

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }

        return Ok(new {
            success = true,
            data = result.Value,
            message = "Task assigned successfully"
        });
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaskStatus(string id, [FromBody] UpdateTaskStatusCommand command)
    {
        if (id != command.TaskId)
            return BadRequest(new {
                success = false,
                message = "ID mismatch",
                errors = new[] { "Task ID in URL does not match ID in request body" }
            });

        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }

        return Ok(new {
            success = true,
            data = result.Value,
            message = "Task status updated successfully"
        });
    }

    [HttpPatch("{id}/start")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartTask(string id)
    {
        var command = new StartTaskCommand { TaskId = id };
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }

        return Ok(new {
            success = true,
            data = result.Value,
            message = "Task started successfully"
        });
    }

    [HttpPatch("{id}/submit-for-review")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitForReview(string id)
    {
        var command = new SubmitForReviewCommand { TaskId = id };
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }

        return Ok(new {
            success = true,
            data = result.Value,
            message = "Task submitted for review successfully"
        });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(string id)
    {
        var command = new DeleteTaskCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }
        
        if (!result.Value)
            return NotFound(new {
                success = false,
                message = $"Task with ID {id} not found.",
                errors = new[] { "Task not found" }
            });
            
        return Ok(new {
            success = true,
            data = (object?)null,
            message = "Task deleted successfully"
        });
    }

    [HttpGet("statuses")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetTaskStatuses()
    {
        var statuses = Enum.GetValues<Domain.Enums.TaskStatus>()
            .Select(status => new { 
                value = GetStatusKebabCase(status), 
                name = status.ToString(),
                displayName = GetStatusDisplayName(status)
            });
        
        return Ok(new { 
            success = true,
            data = statuses,
            message = "Task statuses retrieved successfully" 
        });
    }

    [HttpGet("priorities")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetTaskPriorities()
    {
        var priorities = Enum.GetValues<Domain.Enums.TaskPriority>()
            .Select(priority => new { 
                value = GetPriorityKebabCase(priority), 
                name = priority.ToString(),
                displayName = GetPriorityDisplayName(priority)
            });
        
        return Ok(new { 
            success = true,
            data = priorities,
            message = "Task priorities retrieved successfully" 
        });
    }

    private static string GetStatusKebabCase(Domain.Enums.TaskStatus status)
    {
        return status switch
        {
            Domain.Enums.TaskStatus.Todo => "todo",
            Domain.Enums.TaskStatus.InProgress => "in-progress",
            Domain.Enums.TaskStatus.Review => "review",
            Domain.Enums.TaskStatus.Completed => "completed",
            _ => status.ToString().ToLower()
        };
    }

    private static string GetPriorityKebabCase(Domain.Enums.TaskPriority priority)
    {
        return priority switch
        {
            Domain.Enums.TaskPriority.Low => "low",
            Domain.Enums.TaskPriority.Medium => "medium",
            Domain.Enums.TaskPriority.High => "high",
            Domain.Enums.TaskPriority.Urgent => "urgent",
            _ => priority.ToString().ToLower()
        };
    }

    private static string GetStatusDisplayName(Domain.Enums.TaskStatus status)
    {
        return status switch
        {
            Domain.Enums.TaskStatus.Todo => "To Do",
            Domain.Enums.TaskStatus.InProgress => "In Progress",
            Domain.Enums.TaskStatus.Review => "Review",
            Domain.Enums.TaskStatus.Completed => "Completed",
            _ => status.ToString()
        };
    }

    private static string GetPriorityDisplayName(Domain.Enums.TaskPriority priority)
    {
        return priority switch
        {
            Domain.Enums.TaskPriority.Low => "Low",
            Domain.Enums.TaskPriority.Medium => "Medium", 
            Domain.Enums.TaskPriority.High => "High",
            Domain.Enums.TaskPriority.Urgent => "Urgent",
            _ => priority.ToString()
        };
    }
} 