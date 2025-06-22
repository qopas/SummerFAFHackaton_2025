using Ediki.Api.DTOs.In;
using Ediki.Application.Commands.Auth.UpdateProfile;
using Ediki.Domain.Enums;

namespace Ediki.Api.DTOs.In.Auth;

public class UpdateProfileRequest : IRequestIn<UpdateProfileCommand>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public PreferredRole? PreferredRole { get; set; }

    public UpdateProfileCommand Convert()
    {
        return new UpdateProfileCommand
        {
            FirstName = FirstName,
            LastName = LastName,
            PreferredRole = PreferredRole
        };
    }
} 