using Ediki.Domain.Enums;

namespace Ediki.Application.Features.Invitations.DTOs;

public class InvitationDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string RelatedEntityId { get; set; } = string.Empty; // Project ID or Team ID
    public string RelatedEntityType { get; set; } = string.Empty; // "Project" or "Team"
    public string RelatedEntityName { get; set; } = string.Empty; // Project Title or Team Name
    public string RelatedEntityDescription { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public float Progress { get; set; }
    public bool IsActive { get; set; }
    public string? InvitedBy { get; set; }
    public string? InvitedByName { get; set; }
    public DateTime? InvitedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public bool IsProjectLead { get; set; }
} 