using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;

namespace Ediki.Application.Commands.Auth.UpdateUser;

public class UpdateUserCommandHandler(UserManager<ApplicationUser> userManager) 
    : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null || user.IsDeleted)
            {
                return Result<UserDto>.Failure("User not found");
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;
            
            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;
            
            if (request.PreferredRole.HasValue)
                user.PreferredRole = request.PreferredRole.Value;
            
            if (request.Skills != null)
                user.Skills = request.Skills;
            
            if (!string.IsNullOrEmpty(request.University))
                user.University = request.University;
            
            if (request.GraduationYear.HasValue)
                user.GraduationYear = request.GraduationYear;
            
            if (!string.IsNullOrEmpty(request.Location))
                user.Location = request.Location;
            
            if (request.SocialLinks != null)
                user.SocialLinks = request.SocialLinks;

            user.UpdatedAt = DateTime.UtcNow;

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
                Roles = roles.ToList(),
                CreatedAt = user.CreatedAt,
                PreferredRole = user.PreferredRole,
                Xp = user.Xp,
                Level = user.Level,
                Badges = user.Badges,
                CompletedProjects = user.CompletedProjects,
                Skills = user.Skills,
                University = user.University,
                GraduationYear = user.GraduationYear,
                Location = user.Location,
                SocialLinks = user.SocialLinks
            };

            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
    }
} 