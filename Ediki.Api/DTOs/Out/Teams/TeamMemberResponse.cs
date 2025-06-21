using Ediki.Api.DTOs.Out;
using Ediki.Application.Features.Teams.DTOs;

namespace Ediki.Api.DTOs.Out.Teams;

public class TeamMemberResponse : IResponseOut<TeamMemberDto>
{
    public string Id { get; set; } = string.Empty;
    public string TeamId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public float Progress { get; set; }
    public bool IsActive { get; set; }
    public string? InvitedBy { get; set; }
    public DateTime? InvitedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }

    public TeamMemberResponse() { }

    public TeamMemberResponse(TeamMemberDto result)
    {
        Id = result.Id;
        TeamId = result.TeamId;
        UserId = result.UserId;
        UserName = result.UserName;
        UserEmail = result.UserEmail;
        Role = result.Role;
        JoinedAt = result.JoinedAt;
        Progress = result.Progress;
        IsActive = result.IsActive;
        InvitedBy = result.InvitedBy;
        InvitedAt = result.InvitedAt;
        AcceptedAt = result.AcceptedAt;
    }

    public object? Convert(TeamMemberDto result)
    {
        return new TeamMemberResponse(result);
    }
}