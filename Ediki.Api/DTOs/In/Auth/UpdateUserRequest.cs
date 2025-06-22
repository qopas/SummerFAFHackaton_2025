using Ediki.Application.Commands.Auth.UpdateUser;
using Ediki.Api.DTOs.In;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace Ediki.Api.DTOs.In.Auth;

public class UpdateUserRequest : IRequestIn<UpdateUserCommand>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ProjectRole? PreferredRole { get; set; }
    public List<string>? Skills { get; set; }
    public string? University { get; set; }
    public int? GraduationYear { get; set; }
    public string? Location { get; set; }
    public SocialLinks? SocialLinks { get; set; }

    public UpdateUserCommand Convert()
    {
        return new UpdateUserCommand
        {
            FirstName = FirstName,
            LastName = LastName,
            PreferredRole = PreferredRole,
            Skills = Skills,
            University = University,
            GraduationYear = GraduationYear,
            Location = Location,
            SocialLinks = SocialLinks
        };
    }
} 