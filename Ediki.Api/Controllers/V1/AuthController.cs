using System.Security.Claims;
using Ediki.Api.DTOs.In.Auth;
using Ediki.Api.DTOs.Out.Auth;
using Ediki.Application.Commands.Auth.Login;
using Ediki.Application.DTOs.Auth;
using Ediki.Application.Queries.Auth.GetAllUsers;
using Ediki.Application.Queries.Auth.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SummerFAFHackaton_2025.Controllers.V1;

[Route("api/v1/[controller]")]
public class AuthController : BaseApiController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return await ExecuteAsync<LoginCommand, LoginResponse, LoginResult>(request);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await ExecuteAsync<Ediki.Application.Commands.Auth.Register.RegisterCommand, RegisterResponse, Ediki.Application.DTOs.Auth.RegisterResult>(request);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        var query = new GetUserByIdQuery(userId);
        return await ExecuteQueryAsync<UserResponse, UserDto>(query);
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
