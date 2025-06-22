using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Ediki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Ediki.Infrastructure.Repositories;

public class ProjectMemberRepository(ApplicationDbContext dbContext) : IProjectMemberRepository
{
    public async Task<ProjectMember?> GetByIdAsync(string id)
    {
        return await dbContext.ProjectMembers
            .Include(pm => pm.Project)
            .Include(pm => pm.User)
            .Include(pm => pm.InvitedByUser)
            .FirstOrDefaultAsync(pm => pm.Id == id && !pm.IsDeleted);
    }

    public async Task<ProjectMember?> GetByProjectAndUserAsync(string projectId, string userId)
    {
        return await dbContext.ProjectMembers
            .Include(pm => pm.Project)
            .Include(pm => pm.User)
            .Include(pm => pm.InvitedByUser)
            .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId && !pm.IsDeleted);
    }

    public async Task<List<ProjectMember>> GetProjectMembersAsync(string projectId, bool activeOnly = true)
    {
        var query = dbContext.ProjectMembers
            .Include(pm => pm.User)
            .Include(pm => pm.InvitedByUser)
            .Where(pm => pm.ProjectId == projectId && !pm.IsDeleted);

        if (activeOnly)
            query = query.Where(pm => pm.IsActive);

        return await query.OrderBy(pm => pm.JoinedAt).ToListAsync();
    }

    public async Task<List<ProjectMember>> GetUserProjectsAsync(string userId, bool activeOnly = true)
    {
        var query = dbContext.ProjectMembers
            .Include(pm => pm.Project)
            .Include(pm => pm.InvitedByUser)
            .Where(pm => pm.UserId == userId && !pm.IsDeleted);

        if (activeOnly)
            query = query.Where(pm => pm.IsActive);

        return await query.OrderBy(pm => pm.JoinedAt).ToListAsync();
    }

    public async Task<List<ProjectMember>> GetAllAsync(
        string? projectId = null,
        string? userId = null,
        ProjectRole? role = null,
        bool? isActive = null,
        bool? isProjectLead = null,
        int page = 1,
        int pageSize = 10)
    {
        var query = dbContext.ProjectMembers
            .Include(pm => pm.Project)
            .Include(pm => pm.User)
            .Include(pm => pm.InvitedByUser)
            .Where(pm => !pm.IsDeleted);

        if (!string.IsNullOrEmpty(projectId))
            query = query.Where(pm => pm.ProjectId == projectId);

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(pm => pm.UserId == userId);

        if (role.HasValue)
            query = query.Where(pm => pm.Role == role.Value);

        if (isActive.HasValue)
            query = query.Where(pm => pm.IsActive == isActive.Value);

        if (isProjectLead.HasValue)
            query = query.Where(pm => pm.IsProjectLead == isProjectLead.Value);

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(pm => pm.JoinedAt)
            .ToListAsync();
    }

    public async Task<ProjectMember> AddAsync(ProjectMember projectMember)
    {
        projectMember.CreatedAt = DateTime.UtcNow;
        await dbContext.ProjectMembers.AddAsync(projectMember);
        await dbContext.SaveChangesAsync();
        return projectMember;
    }

    public async Task UpdateAsync(ProjectMember projectMember)
    {
        projectMember.UpdatedAt = DateTime.UtcNow;
        dbContext.Entry(projectMember).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProjectMember projectMember)
    {
        dbContext.ProjectMembers.Remove(projectMember);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await dbContext.ProjectMembers.AnyAsync(pm => pm.Id == id && !pm.IsDeleted);
    }

    public async Task<bool> IsProjectMemberAsync(string projectId, string userId)
    {
        return await dbContext.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId && pm.IsActive && !pm.IsDeleted);
    }

    public async Task<bool> IsProjectLeadAsync(string projectId, string userId)
    {
        return await dbContext.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId && pm.IsProjectLead && pm.IsActive && !pm.IsDeleted);
    }

    public async Task<bool> HasRequiredRoleAsync(string projectId, string userId, ProjectRole requiredRole)
    {
        return await dbContext.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId && pm.Role == requiredRole && pm.IsActive && !pm.IsDeleted);
    }

    public async Task<int> GetMemberCountAsync(string projectId)
    {
        return await dbContext.ProjectMembers
            .CountAsync(pm => pm.ProjectId == projectId && pm.IsActive && !pm.IsDeleted);
    }

    public async Task<int> GetActiveMemberCountAsync(string projectId)
    {
        return await dbContext.ProjectMembers
            .CountAsync(pm => pm.ProjectId == projectId && pm.IsActive && !pm.IsDeleted);
    }

    public async Task<bool> CanUserJoinProjectAsync(string projectId, string userId, ProjectRole role)
    {
        // Check if user is already a member
        var isAlreadyMember = await IsProjectMemberAsync(projectId, userId);
        if (isAlreadyMember)
            return false;

        // Get project details
        var project = await dbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && !p.IsDeleted);

        if (project == null)
            return false;

        // Check if project needs this role
        if (!project.RolesNeeded.Contains(role))
            return false;

        // Check if project has space for more members
        var currentMemberCount = await GetMemberCountAsync(projectId);
        if (currentMemberCount >= project.MaxParticipants)
            return false;

        return true;
    }
} 