using Ediki.Api.DTOs.Out;
using Ediki.Api.DTOs.Out.Teams;
using Ediki.Application.Features.Teams.DTOs;

namespace SummerFAFHackaton_2025.DTOs.Out.Teams;

public class TeamResponse : IResponseOut<TeamDto>
{
    public string Id { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string? Name { get; set; }
    public int MaxMembers { get; set; }
    public bool IsComplete { get; set; }
    public string? InviteCode { get; set; }
    public string? TeamLead { get; set; }
    public int CurrentMemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<TeamMemberResponse> Members { get; set; } = new();

    public TeamResponse() { }

    public TeamResponse(TeamDto result)
    {
        Id = result.Id;
        ProjectId = result.ProjectId;
        Name = result.Name;
        MaxMembers = result.MaxMembers;
        IsComplete = result.IsComplete;
        InviteCode = result.InviteCode;
        TeamLead = result.TeamLead;
        CurrentMemberCount = result.CurrentMemberCount;
        CreatedAt = result.CreatedAt;
        UpdatedAt = result.UpdatedAt;
        Members = result.Members.Select(m => new TeamMemberResponse(m)).ToList();
    }

    public object? Convert(TeamDto result)
    {
        return new TeamResponse(result);
    }
}