using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Commands.Auth.UpdateProfile;

public class UpdateProfileCommand : IRequest<Result<UserDto>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public PreferredRole? PreferredRole { get; set; }
} 