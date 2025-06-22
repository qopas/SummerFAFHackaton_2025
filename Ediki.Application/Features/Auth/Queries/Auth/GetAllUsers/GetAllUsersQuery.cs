using MediatR;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;

namespace Ediki.Application.Queries.Auth.GetAllUsers;

public record GetAllUsersQuery : IRequest<Result<IEnumerable<UserDto>>>;
