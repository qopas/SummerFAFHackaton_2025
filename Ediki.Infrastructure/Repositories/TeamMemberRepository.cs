using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Ediki.Domain.Interfaces;
using Ediki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Infrastructure.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TeamMemberRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TeamMember?> GetByIdAsync(string id)
    {
        return await _dbContext.TeamMembers
            .FirstOrDefaultAsync(tm => tm.Id == id);
    }

    public async Task<TeamMember?> GetByTeamAndUserAsync(string teamId, string userId)
    {
        return await _dbContext.TeamMembers
            .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
    }

    public async Task<List<TeamMember>> GetByTeamIdAsync(string teamId)
    {
        return await _dbContext.TeamMembers
            .Where(tm => tm.TeamId == teamId)
            .ToListAsync();
    }

    public async Task<List<TeamMember>> GetByUserIdAsync(string userId)
    {
        return await _dbContext.TeamMembers
            .Where(tm => tm.UserId == userId && tm.IsActive)
            .OrderByDescending(tm => tm.JoinedAt)
            .ToListAsync();
    }

    public async Task<List<TeamMember>> GetAllAsync(
        string? teamId = null,
        string? userId = null,
        ProjectRole? role = null,
        bool? isActive = null,
        bool? hasPendingInvitation = null,
        int page = 1,
        int pageSize = 10)
    {
        var query = _dbContext.TeamMembers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(teamId))
            query = query.Where(tm => tm.TeamId == teamId);

        if (!string.IsNullOrWhiteSpace(userId))
            query = query.Where(tm => tm.UserId == userId);

        if (isActive.HasValue)
            query = query.Where(tm => tm.IsActive == isActive.Value);

        if (hasPendingInvitation.HasValue)
        {
            if (hasPendingInvitation.Value)
                query = query.Where(tm => tm.InvitedAt != null && tm.AcceptedAt == null);
            else
                query = query.Where(tm => tm.AcceptedAt != null);
        }

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<TeamMember> AddAsync(TeamMember teamMember)
    {
        await _dbContext.TeamMembers.AddAsync(teamMember);
        await _dbContext.SaveChangesAsync();
        return teamMember;
    }

    public async Task UpdateAsync(TeamMember teamMember)
    {
        _dbContext.Entry(teamMember).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TeamMember teamMember)
    {
        _dbContext.TeamMembers.Remove(teamMember);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _dbContext.TeamMembers.AnyAsync(tm => tm.Id == id);
    }

    public async Task<bool> IsUserInTeamAsync(string teamId, string userId)
    {
        return await _dbContext.TeamMembers
            .AnyAsync(tm => tm.TeamId == teamId && tm.UserId == userId && tm.IsActive);
    }

    public async Task<bool> HasPendingInvitationAsync(string teamId, string userId)
    {
        return await _dbContext.TeamMembers
            .AnyAsync(tm => tm.TeamId == teamId && tm.UserId == userId && 
                           tm.InvitedAt != null && tm.AcceptedAt == null);
    }

    public async Task<int> GetActiveTeamMemberCountAsync(string teamId)
    {
        return await _dbContext.TeamMembers
            .CountAsync(tm => tm.TeamId == teamId && tm.IsActive);
    }

    public async Task<List<TeamMember>> GetPendingInvitationsAsync(string userId)
    {
        return await _dbContext.TeamMembers
            .Where(tm => tm.UserId == userId && tm.InvitedAt != null && tm.AcceptedAt == null)
            .OrderByDescending(tm => tm.InvitedAt)
            .ToListAsync();
    }
}