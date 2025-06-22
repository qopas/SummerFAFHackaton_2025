using Ediki.Application.DTOs.Auth;
using Ediki.Api.DTOs.Out;
using Ediki.Domain.Enums;

namespace Ediki.Api.DTOs.Out.Auth;

public class UserResponse : IResponseOut<UserDto>
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public PreferredRole PreferredRole { get; set; } = PreferredRole.NotSet;
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }

    public object? Convert(UserDto result)
    {
        return new UserResponse
        {
            Id = result.Id,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            PreferredRole = result.PreferredRole,
            Roles = result.Roles,
            CreatedAt = result.CreatedAt
        };
    }
}
