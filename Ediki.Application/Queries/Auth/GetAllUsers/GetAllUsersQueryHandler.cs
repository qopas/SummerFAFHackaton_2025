using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ediki.Application.DTOs.Auth;
using Ediki.Application.Queries.Auth.GetAllUsers;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;

namespace Ediki.Application.Queries.Auth.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users.Where(u => !u.IsDeleted).ToListAsync(cancellationToken);
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PreferredRole = user.PreferredRole,
                Roles = roles.ToList(),
                CreatedAt = user.CreatedAt
            });
        }

        return Result<IEnumerable<UserDto>>.Success(userDtos);
    }
}
