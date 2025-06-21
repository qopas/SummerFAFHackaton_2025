using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Ediki.Domain.Interfaces;
using Ediki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Ediki.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project?> GetByIdAsync(string id)
    {
        return await _dbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Project>> GetAllAsync(
        string? searchTerm = null,
        ProjectStatus? status = null,
        Difficulty? difficulty = null,
        ProjectRole? role = null,
        bool? isFeatured = null,
        bool includePrivate = false,
        string? userId = null,
        int page = 1,
        int pageSize = 10)
    {
        var query = _dbContext.Projects.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();
            query = query.Where(p =>
                p.Title.ToLower().Contains(searchTermLower) ||
                p.Description.ToLower().Contains(searchTermLower) ||
                p.ShortDescription.ToLower().Contains(searchTermLower) ||
                p.Tags.Any(t => t.ToLower().Contains(searchTermLower))
            );
        }
            
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);
            
        if (difficulty.HasValue)
            query = query.Where(p => p.Difficulty == difficulty.Value);
            
        if (role.HasValue)
            query = query.Where(p => p.RolesNeeded.Contains(role.Value));
            
        if (isFeatured.HasValue)
            query = query.Where(p => p.IsFeatured == isFeatured.Value);
            
        // Handle private projects
        if (!includePrivate)
        {
            query = !string.IsNullOrEmpty(userId)
                ? query.Where(p => p.IsPublic || p.CreatedById == userId)
                : query.Where(p => p.IsPublic);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Project> AddAsync(Project project)
    {
        await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }

    public async Task UpdateAsync(Project project)
    {
        _dbContext.Entry(project).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project project)
    {
        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _dbContext.Projects.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> IsOwnerAsync(string projectId, string userId)
    {
        return await _dbContext.Projects
            .AnyAsync(p => p.Id == projectId && p.CreatedById == userId);
    }
} 