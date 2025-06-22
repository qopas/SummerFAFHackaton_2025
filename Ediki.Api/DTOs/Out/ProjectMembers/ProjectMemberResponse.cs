using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Api.DTOs.Out;
using Ediki.Domain.Enums;

namespace Ediki.Api.DTOs.Out.ProjectMembers;

public class ProjectMemberResponse : IResponseOut<ProjectMemberDto>
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

    public object? Convert(ProjectMemberDto result)
    {
        return new ProjectMemberResponse
        {
            Id = result.Id,
            ProjectId = result.ProjectId,
            UserId = result.UserId,
            UserName = result.UserName,
            UserEmail = result.UserEmail,
            UserFirstName = result.UserFirstName,
            UserLastName = result.UserLastName,
            UserPreferredRole = result.UserPreferredRole,
            Role = result.Role,
            JoinedAt = result.JoinedAt,
            Progress = result.Progress,
            IsActive = result.IsActive,
            InvitedBy = result.InvitedBy,
            InvitedByName = result.InvitedByName,
            InvitedAt = result.InvitedAt,
            AcceptedAt = result.AcceptedAt,
            IsProjectLead = result.IsProjectLead
        };
    }
} 