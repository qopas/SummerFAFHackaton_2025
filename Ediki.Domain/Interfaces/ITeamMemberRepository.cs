using Ediki.Domain.Entities;
using Ediki.Domain.Enums;

namespace Ediki.Domain.Interfaces;

public interface ITeamMemberRepository
{
    Task<TeamMember?> GetByIdAsync(string id);
    Task<TeamMember?> GetByTeamAndUserAsync(string teamId, string userId);
    Task<List<TeamMember>> GetByTeamIdAsync(string teamId);
    Task<List<TeamMember>> GetByUserIdAsync(string userId);
    Task<List<TeamMember>> GetAllAsync(
        string? teamId = null,
        string? userId = null,
        ProjectRole? role = null,
        bool? isActive = null,
        bool? hasPendingInvitation = null,
        int page = 1,
        int pageSize = 10);
    Task<TeamMember> AddAsync(TeamMember teamMember);
    Task UpdateAsync(TeamMember teamMember);
    Task DeleteAsync(TeamMember teamMember);
    Task<bool> ExistsAsync(string id);
    Task<bool> IsUserInTeamAsync(string teamId, string userId);
    Task<bool> HasPendingInvitationAsync(string teamId, string userId);
    Task<int> GetActiveTeamMemberCountAsync(string teamId);
    Task<List<TeamMember>> GetPendingInvitationsAsync(string userId);
}