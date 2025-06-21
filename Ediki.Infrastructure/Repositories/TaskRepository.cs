using Ediki.Application.Common.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Ediki.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DomainTask = Ediki.Domain.Entities.Task;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Infrastructure.Repositories;

public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    public async System.Threading.Tasks.Task<DomainTask?> GetByIdAsync(string id)
    {
        return await context.Tasks
            .Include(t => t.Assignee)
            .Include(t => t.CreatedByUser)
            .Include(t => t.Sprint)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetBySprintIdAsync(string sprintId)
    {
        return await context.Tasks
            .Where(t => t.SprintId == sprintId)
            .Include(t => t.Assignee)
            .Include(t => t.CreatedByUser)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetByProjectIdAsync(string projectId)
    {
        return await context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Assignee)
            .Include(t => t.CreatedByUser)
            .Include(t => t.Sprint)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetByAssigneeIdAsync(string assigneeId)
    {
        return await context.Tasks
            .Where(t => t.AssigneeId == assigneeId)
            .Include(t => t.Assignee)
            .Include(t => t.CreatedByUser)
            .Include(t => t.Sprint)
            .Include(t => t.Project)
            .OrderBy(t => t.DueDate ?? DateTime.MaxValue)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetTasksWithFiltersAsync(string? projectId = null, string? sprintId = null, string? assigneeId = null,
        TaskStatus? status = null, TaskPriority? priority = null, DateTime? dueDateFrom = null, DateTime? dueDateTo = null)
    {
        var query = context.Tasks
            .Include(t => t.Assignee)
            .Include(t => t.CreatedByUser)
            .Include(t => t.Sprint)
            .Include(t => t.Project)
            .AsQueryable();

        if (!string.IsNullOrEmpty(projectId))
            query = query.Where(t => t.ProjectId == projectId);

        if (!string.IsNullOrEmpty(sprintId))
            query = query.Where(t => t.SprintId == sprintId);

        if (!string.IsNullOrEmpty(assigneeId))
            query = query.Where(t => t.AssigneeId == assigneeId);

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        if (dueDateFrom.HasValue)
            query = query.Where(t => t.DueDate >= dueDateFrom.Value);

        if (dueDateTo.HasValue)
            query = query.Where(t => t.DueDate <= dueDateTo.Value);

        return await query.OrderBy(t => t.CreatedAt).ToListAsync();
    }
    
    
    public async System.Threading.Tasks.Task<DomainTask> CreateAsync(DomainTask task)
    {
        context.Tasks.Add(task);
        await context.SaveChangesAsync();
        return task;
    }

    public async System.Threading.Tasks.Task<DomainTask> UpdateAsync(DomainTask task)
    {
        task.UpdatedAt = DateTime.UtcNow;
        
        if (task.Status == TaskStatus.Completed && task.CompletedAt == null)
            task.CompletedAt = DateTime.UtcNow;
        else if (task.Status != TaskStatus.Completed)
            task.CompletedAt = null;
            
        context.Tasks.Update(task);
        await context.SaveChangesAsync();
        return task;
    }

    public async System.Threading.Tasks.Task<bool> DeleteAsync(string id)
    {
        var task = await context.Tasks.FindAsync(id);
        if (task == null)
            return false;

        context.Tasks.Remove(task);
        await context.SaveChangesAsync();
        return true;
    }

    public async System.Threading.Tasks.Task<bool> ExistsAsync(string id)
    {
        return await context.Tasks.AnyAsync(t => t.Id == id);
    }

    public async System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetUserTasksAsync(string userId)
    {
        return await context.Tasks
            .Where(t => t.AssigneeId == userId || t.CreatedBy == userId)
            .Include(t => t.Assignee)
            .Include(t => t.CreatedByUser)
            .Include(t => t.Sprint)
            .Include(t => t.Project)
            .OrderBy(t => t.DueDate ?? DateTime.MaxValue)
            .ToListAsync();
    }
} 