using Ediki.Application.Commands.Auth.Register;
using Ediki.Api.DTOs.In;

namespace Ediki.Api.DTOs.In.Auth;

public class RegisterRequest : IRequestIn<RegisterCommand>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = "User";

    public RegisterCommand Convert()
    {
        return new RegisterCommand(Email, Password, ConfirmPassword, FirstName, LastName, Role);
    }
}
