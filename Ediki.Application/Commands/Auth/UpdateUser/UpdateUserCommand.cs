using MediatR;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace Ediki.Application.Commands.Auth.UpdateUser;

public class UpdateUserCommand : IRequest<Result<UserDto>>
{
    public string UserId { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ProjectRole? PreferredRole { get; set; }
    public List<string>? Skills { get; set; }
    public string? University { get; set; }
    public int? GraduationYear { get; set; }
    public string? Location { get; set; }
    public SocialLinks? SocialLinks { get; set; }
} 