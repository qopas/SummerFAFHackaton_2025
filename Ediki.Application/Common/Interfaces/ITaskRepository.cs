using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;
using DomainTask = Ediki.Domain.Entities.Task;

namespace Ediki.Application.Common.Interfaces;

public interface ITaskRepository
{
    System.Threading.Tasks.Task<DomainTask?> GetByIdAsync(string id);
    System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetBySprintIdAsync(string sprintId);
    System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetByProjectIdAsync(string projectId);
    System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetByAssigneeIdAsync(string assigneeId);
    System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetTasksWithFiltersAsync(
        string? projectId = null,
        string? sprintId = null,
        string? assigneeId = null,
        TaskStatus? status = null,
        TaskPriority? priority = null,
        DateTime? dueDateFrom = null,
        DateTime? dueDateTo = null);
    System.Threading.Tasks.Task<DomainTask> CreateAsync(DomainTask task);
    System.Threading.Tasks.Task<DomainTask> UpdateAsync(DomainTask task);
    System.Threading.Tasks.Task<bool> DeleteAsync(string id);
    System.Threading.Tasks.Task<bool> ExistsAsync(string id);
    System.Threading.Tasks.Task<IEnumerable<DomainTask>> GetUserTasksAsync(string userId);
} 