using Ediki.Domain.Enums;

namespace Ediki.Application.Features.Teams.DTOs;

public class TeamDto
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
    public List<TeamMemberDto> Members { get; set; } = new();
}