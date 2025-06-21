using Ediki.Application.DTOs.Auth;
using Ediki.Api.DTOs.Out;

namespace Ediki.Api.DTOs.Out.Auth;

public class LoginResponse : IResponseOut<LoginResult>
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserResponse User { get; set; } = null!;

    public object? Convert(LoginResult result)
    {
        return new LoginResponse
        {
            Token = result.Token,
            RefreshToken = result.RefreshToken,
            ExpiresAt = result.ExpiresAt,
            User = new UserResponse().Convert(result.User) as UserResponse ?? new UserResponse()
        };
    }
}
