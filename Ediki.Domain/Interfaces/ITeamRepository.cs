using Ediki.Domain.Entities;

namespace Ediki.Domain.Interfaces;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(string id);
    Task<Team?> GetByProjectIdAsync(string projectId);
    Task<Team?> GetByInviteCodeAsync(string inviteCode);
    Task<List<Team>> GetAllAsync(
        string? searchTerm = null,
        string? projectId = null,
        bool? isComplete = null,
        string? userId = null,
        int page = 1,
        int pageSize = 10);
    Task<Team> AddAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(Team team);
    Task<bool> ExistsAsync(string id);
    Task<bool> ExistsByProjectIdAsync(string projectId);
    Task<bool> IsTeamLeadAsync(string teamId, string userId);
    Task<int> GetMemberCountAsync(string teamId);
}