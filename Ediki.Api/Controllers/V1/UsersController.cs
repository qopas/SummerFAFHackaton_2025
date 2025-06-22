using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SummerFAFHackaton_2025.Controllers;
using Ediki.Application.Queries.Users.SearchUsers;
using Ediki.Application.DTOs.Auth;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(IMediator mediator) : BaseApiController(mediator)
{
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
} 