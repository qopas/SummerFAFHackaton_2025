using Ediki.Application.DTOs.Auth;
using Ediki.Api.DTOs.Out;

namespace Ediki.Api.DTOs.Out.Auth;

public class RegisterResponse : IResponseOut<RegisterResult>
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public object? Convert(RegisterResult result)
    {
        return new RegisterResponse
        {
            UserId = result.UserId,
            Email = result.Email,
            Message = result.Message
        };
    }
}
