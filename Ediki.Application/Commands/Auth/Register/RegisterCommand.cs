using MediatR;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Enums;
using Ediki.Domain.ValueObjects;

namespace Ediki.Application.Commands.Auth.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string Role,
    ProjectRole PreferredRole = ProjectRole.NotSet,
    int Xp = 0,
    int Level = 1,
    List<string>? Badges = null,
    int CompletedProjects = 0,
    List<string>? Skills = null,
    string? University = null,
    int? GraduationYear = null,
    string? Location = null,
    SocialLinks? SocialLinks = null
) : IRequest<Result<RegisterResult>>;
