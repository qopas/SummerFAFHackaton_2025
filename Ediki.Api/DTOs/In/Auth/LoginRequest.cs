using Ediki.Application.Commands.Auth.Login;
using Ediki.Api.DTOs.In;

namespace Ediki.Api.DTOs.In.Auth;

public class LoginRequest : IRequestIn<LoginCommand>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public LoginCommand Convert()
    {
        return new LoginCommand(Email, Password);
    }
}
