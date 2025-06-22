using MediatR;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;

namespace Ediki.Application.Commands.Auth.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<LoginResult>>;
