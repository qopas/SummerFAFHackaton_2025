using MediatR;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;

namespace Ediki.Application.Commands.Auth.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string Role
) : IRequest<Result<RegisterResult>>;
