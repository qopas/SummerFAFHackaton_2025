using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ediki.Application.DTOs.Auth;
using Ediki.Application.Queries.Auth.GetAllUsers;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;

namespace Ediki.Application.Queries.Auth.GetAllUsers;

public class GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
{
    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.Where(u => !u.IsDeleted).ToListAsync(cancellationToken);
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                CreatedAt = user.CreatedAt,
                
                // New fields
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
            });
        }

        return Result<IEnumerable<UserDto>>.Success(userDtos);
    }
}
