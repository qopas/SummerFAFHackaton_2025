using Ediki.Domain.Common;

namespace Ediki.Domain.Entities;

public class Team : BaseEntity
{
    public string ProjectId { get; private set; } = string.Empty;
    public string? Name { get; private set; }
    public int MaxMembers { get; private set; }
    public bool IsComplete { get; private set; }
    public string? InviteCode { get; private set; }
    public string? TeamLead { get; private set; }

    private Team() { }

    public static Team Create(
        string projectId,
        string? name = null,
        int maxMembers = 6,
        string? teamLead = null,
        string? inviteCode = null)
    {
        var team = new Team
        {
            ProjectId = projectId,
            Name = name,
            MaxMembers = maxMembers,
            IsComplete = false,
            InviteCode = inviteCode,
            TeamLead = teamLead
        };

        return team;
    }

    public void UpdateName(string? name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetComplete(bool isComplete)
    {
        IsComplete = isComplete;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMaxMembers(int maxMembers)
    {
        if (maxMembers <= 0)
            throw new ArgumentException("Max members must be greater than 0");

        MaxMembers = maxMembers;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetTeamLead(string? teamLead)
    {
        TeamLead = teamLead;
        UpdatedAt = DateTime.UtcNow;
    }

    public void GenerateInviteCode()
    {
        InviteCode = Guid.NewGuid().ToString("N")[..8].ToUpper();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveInviteCode()
    {
        InviteCode = null;
        UpdatedAt = DateTime.UtcNow;
    }
}