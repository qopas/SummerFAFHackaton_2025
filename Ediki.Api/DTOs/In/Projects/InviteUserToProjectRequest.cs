using Ediki.Application.Features.ProjectMembers.Commands.InviteUserToProject;
using Ediki.Domain.Enums;

namespace Ediki.Api.DTOs.In.Projects;

public class InviteUserToProjectRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public InviteUserToProjectCommand ToCommand(string projectId)
    {
        // Parse role string to enum
        ProjectRole roleEnum = ProjectRole.Developer; // Default value
        
        if (Enum.TryParse<ProjectRole>(Role, true, out var parsedRole))
        {
            roleEnum = parsedRole;
        }
        else
        {
            // Handle string mappings for common role names
            roleEnum = Role.ToLower() switch
            {
                "notset" => ProjectRole.Developer, // Default to Developer for NotSet
                "developer" => ProjectRole.Developer,
                "designer" => ProjectRole.Designer,
                "analyst" => ProjectRole.Analyst,
                "legal" => ProjectRole.Legal,
                "marketing" => ProjectRole.Marketing,
                _ => ProjectRole.Developer
            };
        }

        return new InviteUserToProjectCommand
        {
            ProjectId = projectId,
            UserId = UserId,
            Role = roleEnum
        };
    }
} 