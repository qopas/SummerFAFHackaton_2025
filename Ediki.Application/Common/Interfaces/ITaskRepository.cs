using Ediki.Domain.Enums;
using TaskStatus = Ediki.Domain.Enums.TaskStatus;

namespace Ediki.Application.Common.Interfaces;

public interface ITaskRepository
{
    Task<Domain.Entities.Task?> GetByIdAsync(string id);
    Task<IEnumerable<Domain.Entities.Task>> GetBySprintIdAsync(string sprintId);
    Task<IEnumerable<Domain.Entities.Task>> GetByProjectIdAsync(string projectId);
    Task<IEnumerable<Domain.Entities.Task>> GetByAssigneeIdAsync(string assigneeId);
    Task<IEnumerable<Domain.Entities.Task>> GetTasksWithFiltersAsync(
        string? projectId = null,
        string? sprintId = null,
        string? assigneeId = null,
        TaskStatus? status = null,
        TaskPriority? priority = null,
        DateTime? dueDateFrom = null,
        DateTime? dueDateTo = null);
    Task<Domain.Entities.Task> CreateAsync(Domain.Entities.Task task);
    Task<Domain.Entities.Task> UpdateAsync(Domain.Entities.Task task);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<IEnumerable<Domain.Entities.Task>> GetUserTasksAsync(string userId);
} 