using Ediki.Application.Features.Notifications.Commands.CreateMention;
using Ediki.Application.Features.Notifications.Commands.MarkAsRead;
using Ediki.Application.Features.Notifications.DTOs;
using Ediki.Application.Features.Notifications.Queries.GetUserNotifications;
using Ediki.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SummerFAFHackaton_2025.Controllers;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(List<NotificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] bool? isRead = null,
        [FromQuery] NotificationType? type = null,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        var query = new GetUserNotificationsQuery
        {
            IsRead = isRead,
            Type = type,
            Skip = skip,
            Take = take
        };

        var result = await _mediator.Send(query);
        
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
            message = "Notifications retrieved successfully"
        });
    }

    [HttpPost("mention")]
    [ProducesResponseType(typeof(List<NotificationDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMention([FromBody] CreateMentionCommand command)
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

        return CreatedAtAction(nameof(GetNotifications), null, new {
            success = true,
            data = result.Value,
            message = "Mentions created successfully"
        });
    }

    [HttpPatch("{id}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var command = new MarkAsReadCommand { NotificationId = id };
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { 
                success = false,
                message = result.Error, 
                errors = result.Errors 
            });
        }

        return Ok(new {
            success = true,
            data = result.Value,
            message = "Notification marked as read"
        });
    }
} 