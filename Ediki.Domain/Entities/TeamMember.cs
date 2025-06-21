using Ediki.Domain.Common;

namespace Ediki.Domain.Entities;

public class TeamMember : BaseEntity
{
    public string TeamId { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public DateTime JoinedAt { get; private set; }
    public float Progress { get; private set; }
    public bool IsActive { get; private set; }
    public string? InvitedBy { get; private set; }
    public DateTime? InvitedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }

    private TeamMember() { }

    public static TeamMember Create(
        string teamId,
        string userId,
        string role,
        string? invitedBy = null)
    {
        var teamMember = new TeamMember
        {
            TeamId = teamId,
            UserId = userId,
            Role = role,
            JoinedAt = DateTime.UtcNow,
            Progress = 0.0f,
            IsActive = true,
            InvitedBy = invitedBy,
            InvitedAt = invitedBy != null ? DateTime.UtcNow : null,
            AcceptedAt = invitedBy == null ? DateTime.UtcNow : null // If no invitation, auto-accept
        };

        return teamMember;
    }

    public static TeamMember CreateInvitation(
        string teamId,
        string userId,
        string role,
        string invitedBy)
    {
        var teamMember = new TeamMember
        {
            TeamId = teamId,
            UserId = userId,
            Role = role,
            JoinedAt = DateTime.UtcNow,
            Progress = 0.0f,
            IsActive = false, // Not active until invitation is accepted
            InvitedBy = invitedBy,
            InvitedAt = DateTime.UtcNow,
            AcceptedAt = null
        };

        return teamMember;
    }

    public void AcceptInvitation()
    {
        if (AcceptedAt.HasValue)
            throw new InvalidOperationException("Invitation has already been accepted");

        IsActive = true;
        AcceptedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProgress(float progress)
    {
        if (progress < 0 || progress > 1)
            throw new ArgumentException("Progress must be between 0.0 and 1.0");

        Progress = progress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role cannot be empty");

        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Leave()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}