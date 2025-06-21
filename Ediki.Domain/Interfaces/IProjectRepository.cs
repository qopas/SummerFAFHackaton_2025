using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace Ediki.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(string id);
    Task<List<Project>> GetAllAsync(
        string? searchTerm = null,
        ProjectStatus? status = null,
        Difficulty? difficulty = null,
        ProjectRole? role = null,
        bool? isFeatured = null,
        bool includePrivate = false,
        string? userId = null,
        int page = 1,
        int pageSize = 10);
    Task<Project> AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(Project project);
    Task<bool> ExistsAsync(string id);
    Task<bool> IsOwnerAsync(string projectId, string userId);
} 