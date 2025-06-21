using Ediki.Domain.Entities;
using Ediki.Domain.Interfaces;
using Ediki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TeamRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Team?> GetByIdAsync(string id)
    {
        return await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Team?> GetByProjectIdAsync(string projectId)
    {
        return await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.ProjectId == projectId);
    }

    public async Task<Team?> GetByInviteCodeAsync(string inviteCode)
    {
        return await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.InviteCode == inviteCode);
    }

    public async Task<List<Team>> GetAllAsync(
        string? searchTerm = null,
        string? projectId = null,
        bool? isComplete = null,
        string? userId = null,
        int page = 1,
        int pageSize = 10)
    {
        var query = _dbContext.Teams.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();
            query = query.Where(t => t.Name.ToLower().Contains(searchTermLower));
        }

        if (!string.IsNullOrWhiteSpace(projectId))
            query = query.Where(t => t.ProjectId == projectId);

        if (isComplete.HasValue)
            query = query.Where(t => t.IsComplete == isComplete.Value);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(t => t.TeamLead == userId ||
                _dbContext.TeamMembers.Any(tm => tm.TeamId == t.Id && tm.UserId == userId && tm.IsActive));
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Team> AddAsync(Team team)
    {
        team.CreatedAt = DateTime.UtcNow;
        await _dbContext.Teams.AddAsync(team);
        await _dbContext.SaveChangesAsync();
        return team;
    }

    public async Task UpdateAsync(Team team)
    {
        team.UpdatedAt = DateTime.UtcNow;
        _dbContext.Entry(team).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Team team)
    {
        _dbContext.Teams.Remove(team);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _dbContext.Teams.AnyAsync(t => t.Id == id);
    }

    public async Task<bool> ExistsByProjectIdAsync(string projectId)
    {
        return await _dbContext.Teams.AnyAsync(t => t.ProjectId == projectId);
    }

    public async Task<bool> IsTeamLeadAsync(string teamId, string userId)
    {
        return await _dbContext.Teams
            .AnyAsync(t => t.Id == teamId && t.TeamLead == userId);
    }

    public async Task<int> GetMemberCountAsync(string teamId)
    {
        return await _dbContext.TeamMembers
            .CountAsync(tm => tm.TeamId == teamId && tm.IsActive);
    }
}