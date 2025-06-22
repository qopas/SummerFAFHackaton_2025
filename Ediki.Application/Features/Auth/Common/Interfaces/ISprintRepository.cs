using Ediki.Domain.Entities;
using Ediki.Domain.Enums;

namespace Ediki.Application.Common.Interfaces;

public interface ISprintRepository
{
    Task<Sprint?> GetByIdAsync(string id);
    Task<IEnumerable<Sprint>> GetByProjectIdAsync(string projectId);
    Task<Sprint?> GetActiveSprintByProjectIdAsync(string projectId);
    Task<IEnumerable<Sprint>> GetSprintsByStatusAsync(string projectId, SprintStatus status);
    Task<Sprint> CreateAsync(Sprint sprint);
    Task<Sprint> UpdateAsync(Sprint sprint);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<int> GetNextOrderAsync(string projectId);
} 