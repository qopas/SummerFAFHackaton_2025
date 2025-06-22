using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ediki.Application.DTOs.Auth;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using Ediki.Application.Interfaces;

namespace Ediki.Application.Queries.Users.SearchUsers;

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public SearchUsersQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _userManager.Users.Where(u => !u.IsDeleted);

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(u => 
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm) ||
                    u.Email!.ToLower().Contains(searchTerm) ||
                    (u.FirstName + " " + u.LastName).ToLower().Contains(searchTerm));
            }

            // Exclude users who are already members of the specified project
            if (!string.IsNullOrWhiteSpace(request.ExcludeProjectId))
            {
                var projectMemberUserIds = await _context.ProjectMembers
                    .Where(pm => pm.ProjectId == request.ExcludeProjectId && pm.IsActive)
                    .Select(pm => pm.UserId)
                    .ToListAsync(cancellationToken);

                query = query.Where(u => !projectMemberUserIds.Contains(u.Id));
            }

            // Apply pagination
            var users = await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

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
        catch (Exception ex)
        {
            return Result<IEnumerable<UserDto>>.Failure($"Error searching users: {ex.Message}");
        }
    }
} 