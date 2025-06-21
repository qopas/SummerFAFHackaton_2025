using MediatR;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;

namespace Ediki.Application.Queries.Auth.GetUserById;

public record GetUserByIdQuery(string UserId) : IRequest<Result<UserDto>>;
