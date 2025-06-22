using Ediki.Domain.Enums;

namespace Ediki.Application.Features.ProjectMembers.DTOs;

public class ProjectMemberDto
{
    public string Id { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserFirstName { get; set; } = string.Empty;
    public string UserLastName { get; set; } = string.Empty;
    public PreferredRole UserPreferredRole { get; set; } = PreferredRole.NotSet;
    public ProjectRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public float Progress { get; set; }
    public bool IsActive { get; set; }
    public string? InvitedBy { get; set; }
    public string? InvitedByName { get; set; }
    public DateTime? InvitedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public bool IsProjectLead { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectDescription { get; set; } = string.Empty;
} 