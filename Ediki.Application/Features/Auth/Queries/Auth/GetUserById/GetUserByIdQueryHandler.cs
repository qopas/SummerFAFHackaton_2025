using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Application.DTOs.Auth;
using Ediki.Application.Queries.Auth.GetUserById;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;

namespace Ediki.Application.Queries.Auth.GetUserById;

public class GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null || user.IsDeleted)
        {
            return Result<UserDto>.Failure("User not found");
        }

        var roles = await userManager.GetRolesAsync(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PreferredRole = user.PreferredRole,
            Roles = roles.ToList(),
            CreatedAt = user.CreatedAt
        };

        return Result<UserDto>.Success(userDto);
    }
}
