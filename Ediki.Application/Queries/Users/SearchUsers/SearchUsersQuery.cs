using MediatR;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;

namespace Ediki.Application.Queries.Users.SearchUsers;

public class SearchUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
{
    public string SearchTerm { get; set; } = string.Empty;
    public string? ExcludeProjectId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 