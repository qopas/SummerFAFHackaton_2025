using System.Security.Claims;
using Ediki.Api.DTOs.In.Auth;
using Ediki.Api.DTOs.Out.Auth;
using Ediki.Application.Commands.Auth.Login;
using Ediki.Application.Commands.Auth.UpdateProfile;
using Ediki.Application.DTOs.Auth;
using Ediki.Application.Queries.Auth.GetAllUsers;
using Ediki.Application.Queries.Auth.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SummerFAFHackaton_2025.Controllers.V1;

[Route("api/v1/[controller]")]
public class AuthController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            // Проверяем, что request не null и содержит данные
            if (request == null)
            {
                return BadRequest(new { 
                    success = false,
                    message = "Request body is null",
                    errors = new[] { "Invalid request data" }
                });
            }

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { 
                    success = false,
                    message = "Email and password are required",
                    errors = new[] { "Email and password fields cannot be empty" }
                });
            }

            var command = request.Convert();
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error,
                    errors = result.Errors 
                });
            }

            var response = new LoginResponse();
            return Ok(new { 
                success = true,
                data = response.Convert(result.Value),
                message = "Login successful" 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false,
                message = "An error occurred during login",
                errors = new[] { ex.Message },
                stackTrace = ex.StackTrace 
            });
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var command = request.Convert();
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error,
                    errors = result.Errors 
                });
            }

            var response = new RegisterResponse();
            return Ok(new { 
                success = true,
                data = response.Convert(result.Value),
                message = "Registration successful" 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false,
                message = "An error occurred during registration",
                errors = new[] { ex.Message } 
            });
        }
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        var query = new GetUserByIdQuery(userId);
        return await ExecuteQueryAsync<UserResponse, UserDto>(query);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateProfileRequest request)
    {
        try
        {
            var command = request.Convert();
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error,
                    errors = result.Errors 
                });
            }

            var response = new UserResponse();
            return Ok(new { 
                success = true,
                data = response.Convert(result.Value),
                message = "Profile updated successfully" 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                success = false,
                message = "An error occurred during profile update",
                errors = new[] { ex.Message } 
            });
        }
    }

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        return await ExecuteListQueryAsync<UserResponse, UserDto>(query);
    }

    [HttpGet("users/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var query = new GetUserByIdQuery(id);
        return await ExecuteQueryAsync<UserResponse, UserDto>(query);
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }
}
