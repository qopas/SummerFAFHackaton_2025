using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SummerFAFHackaton_2025.Controllers;
using Ediki.Application.Queries.Users.SearchUsers;
using Ediki.Application.DTOs.Auth;
using Ediki.Application.Features.ProjectMembers.Queries.GetPendingInvitations;
using Ediki.Application.Interfaces;
using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Application.Features.ProjectMembers.Commands.AcceptInvitation;
using Ediki.Application.Features.Teams.Commands.AcceptTeamInvitation;
using Ediki.Application.Features.Teams.DTOs;
using Ediki.Application.Features.Invitations.Queries.GetAllPendingInvitations;
using Ediki.Application.Features.Invitations.DTOs;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(IMediator mediator, ICurrentUserService currentUserService) : BaseApiController(mediator)
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string search = "",
        [FromQuery] string? excludeProjectId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            Console.WriteLine($"🔍 Searching users with term: '{search}', excluding project: {excludeProjectId}");
            
            var query = new SearchUsersQuery
            {
                SearchTerm = search,
                ExcludeProjectId = excludeProjectId,
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error, 
                    errors = result.Errors,
                    timestamp = DateTime.UtcNow
                });
            }

            Console.WriteLine($"✅ Found {result.Value.Count()} users");

            return Ok(new {
                success = true,
                data = result.Value,
                message = $"Found {result.Value.Count()} users",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error searching users: {ex.Message}");
            return StatusCode(500, new {
                success = false,
                message = "Error searching users",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("invitations")]
    [ProducesResponseType(typeof(IEnumerable<InvitationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPendingInvitations()
    {
        try
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new {
                    success = false,
                    message = "User not authenticated",
                    errors = new[] { "Authentication required" },
                    timestamp = DateTime.UtcNow
                });
            }

            var query = new GetAllPendingInvitationsQuery { UserId = currentUserId };
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error, 
                    errors = result.Errors,
                    timestamp = DateTime.UtcNow
                });
            }

            return Ok(new {
                success = true,
                data = result.Value,
                message = $"Found {result.Value.Count} pending invitations",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Error retrieving pending invitations",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("invitations/{invitationId}/accept")]
    [ProducesResponseType(typeof(ProjectMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptInvitation(string invitationId)
    {
        try
        {
            var command = new AcceptInvitationCommand { InvitationId = invitationId };
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error, 
                    errors = result.Errors,
                    timestamp = DateTime.UtcNow
                });
            }

            return Ok(new {
                success = true,
                data = result.Value,
                message = "Invitation accepted successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Error accepting invitation",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("team-invitations/{invitationId}/accept")]
    [ProducesResponseType(typeof(TeamMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcceptTeamInvitation(string invitationId)
    {
        try
        {
            var command = new AcceptTeamInvitationCommand { InvitationId = invitationId };
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error, 
                    errors = result.Errors,
                    timestamp = DateTime.UtcNow
                });
            }

            return Ok(new {
                success = true,
                data = result.Value,
                message = "Team invitation accepted successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Error accepting team invitation",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }
} 