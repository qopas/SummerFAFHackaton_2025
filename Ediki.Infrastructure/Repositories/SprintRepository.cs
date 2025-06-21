using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Ediki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Infrastructure.Repositories;

public class SprintRepository : ISprintRepository
{
    private readonly ApplicationDbContext _context;

    public SprintRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Sprint?> GetByIdAsync(string id)
    {
        return await _context.Sprints
            .Include(s => s.Tasks)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sprint>> GetByProjectIdAsync(string projectId)
    {
        return await _context.Sprints
            .Where(s => s.ProjectId == projectId)
            .Include(s => s.Tasks)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<Sprint?> GetActiveSprintByProjectIdAsync(string projectId)
    {
        return await _context.Sprints
            .Include(s => s.Tasks)
            .FirstOrDefaultAsync(s => s.ProjectId == projectId && s.Status == SprintStatus.Active);
    }

    public async Task<IEnumerable<Sprint>> GetSprintsByStatusAsync(string projectId, SprintStatus status)
    {
        return await _context.Sprints
            .Where(s => s.ProjectId == projectId && s.Status == status)
            .Include(s => s.Tasks)
            .OrderBy(s => s.Order)
            .ToListAsync();
    }

    public async Task<Sprint> CreateAsync(Sprint sprint)
    {
        _context.Sprints.Add(sprint);
        await _context.SaveChangesAsync();
        return sprint;
    }

    public async Task<Sprint> UpdateAsync(Sprint sprint)
    {
        sprint.UpdatedAt = DateTime.UtcNow;
        _context.Sprints.Update(sprint);
        await _context.SaveChangesAsync();
        return sprint;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var sprint = await _context.Sprints.FindAsync(id);
        if (sprint == null)
            return false;

        _context.Sprints.Remove(sprint);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Sprints.AnyAsync(s => s.Id == id);
    }

    public async Task<int> GetNextOrderAsync(string projectId)
    {
        var maxOrder = await _context.Sprints
            .Where(s => s.ProjectId == projectId)
            .MaxAsync(s => (int?)s.Order);
        
        return (maxOrder ?? 0) + 1;
    }
} 