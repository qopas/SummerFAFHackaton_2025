using Ediki.Application.DTOs.Auth;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ediki.Application.Commands.Auth.UpdateProfile;

public class UpdateProfileCommandHandler(
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateProfileCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var user = await userManager.FindByIdAsync(userId);
        if (user == null || user.IsDeleted)
        {
            return Result<UserDto>.Failure("User not found");
        }

        if (request.FirstName != null)
        {
            user.FirstName = request.FirstName;
        }

        if (request.LastName != null)
        {
            user.LastName = request.LastName;
        }

        if (request.PreferredRole.HasValue)
        {
            user.PreferredRole = request.PreferredRole.Value;
        }

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result<UserDto>.Failure(errors);
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