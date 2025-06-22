using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace Ediki.Application.Common.Interfaces;

public interface IProjectMemberRepository
{
    Task<ProjectMember?> GetByIdAsync(string id);
    Task<ProjectMember?> GetByProjectAndUserAsync(string projectId, string userId);
    Task<List<ProjectMember>> GetProjectMembersAsync(string projectId, bool activeOnly = true);
    Task<List<ProjectMember>> GetUserProjectsAsync(string userId, bool activeOnly = true);
    Task<List<ProjectMember>> GetAllAsync(
        string? projectId = null,
        string? userId = null,
        ProjectRole? role = null,
        bool? isActive = null,
        bool? isProjectLead = null,
        int page = 1,
        int pageSize = 10);
    Task<ProjectMember> AddAsync(ProjectMember projectMember);
    Task UpdateAsync(ProjectMember projectMember);
    Task DeleteAsync(ProjectMember projectMember);
    Task<bool> ExistsAsync(string id);
    Task<bool> IsProjectMemberAsync(string projectId, string userId);
    Task<bool> IsProjectLeadAsync(string projectId, string userId);
    Task<bool> HasRequiredRoleAsync(string projectId, string userId, ProjectRole requiredRole);
    Task<int> GetMemberCountAsync(string projectId);
    Task<int> GetActiveMemberCountAsync(string projectId);
    Task<bool> CanUserJoinProjectAsync(string projectId, string userId, ProjectRole role);
    Task<List<ProjectMember>> GetPendingInvitationsAsync(string userId);
} 