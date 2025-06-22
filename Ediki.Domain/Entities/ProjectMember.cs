using Ediki.Domain.Common;
using Ediki.Domain.Enums;

namespace Ediki.Domain.Entities;

public class ProjectMember : BaseEntity
{
    public string ProjectId { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    public ProjectRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public float Progress { get; private set; }
    public bool IsActive { get; private set; }
    public string? InvitedBy { get; private set; }
    public DateTime? InvitedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public bool IsProjectLead { get; private set; }

    // Navigation properties
    public Project Project { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ApplicationUser? InvitedByUser { get; set; }

    private ProjectMember() { }

    public static ProjectMember Create(
        string projectId,
        string userId,
        ProjectRole role,
        bool isProjectLead = false,
        string? invitedBy = null)
    {
        var projectMember = new ProjectMember
        {
            ProjectId = projectId,
            UserId = userId,
            Role = role,
            JoinedAt = DateTime.UtcNow,
            Progress = 0.0f,
            IsActive = true,
            IsProjectLead = isProjectLead,
            InvitedBy = invitedBy,
            InvitedAt = invitedBy != null ? DateTime.UtcNow : null,
            AcceptedAt = invitedBy == null ? DateTime.UtcNow : null // If no invitation, auto-accept
        };

        return projectMember;
    }

    public static ProjectMember CreateInvitation(
        string projectId,
        string userId,
        ProjectRole role,
        string invitedBy)
    {
        var projectMember = new ProjectMember
        {
            ProjectId = projectId,
            UserId = userId,
            Role = role,
            JoinedAt = DateTime.UtcNow,
            Progress = 0.0f,
            IsActive = false, // Not active until invitation is accepted
            IsProjectLead = false,
            InvitedBy = invitedBy,
            InvitedAt = DateTime.UtcNow,
            AcceptedAt = null
        };

        return projectMember;
    }

    public void AcceptInvitation()
    {
        if (InvitedBy == null)
            throw new InvalidOperationException("This is not an invitation");

        IsActive = true;
        AcceptedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProgress(float progress)
    {
        if (progress < 0 || progress > 100)
            throw new ArgumentException("Progress must be between 0 and 100");

        Progress = progress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAsProjectLead()
    {
        IsProjectLead = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveProjectLead()
    {
        IsProjectLead = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRole(ProjectRole newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }
} 