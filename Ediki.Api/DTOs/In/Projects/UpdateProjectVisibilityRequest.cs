using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Projects.Commands.UpdateProjectVisibility;

namespace SummerFAFHackaton_2025.DTOs.In.Projects;

public class UpdateProjectVisibilityRequest : IRequestIn<UpdateProjectVisibilityCommand>
{
    public string ProjectId { get; set; } = string.Empty;
    public bool IsPublic { get; set; }

    public UpdateProjectVisibilityRequest() { }

    public UpdateProjectVisibilityRequest(string projectId, bool isPublic)
    {
        ProjectId = projectId;
        IsPublic = isPublic;
    }

    public UpdateProjectVisibilityCommand Convert()
    {
        return new UpdateProjectVisibilityCommand(ProjectId, IsPublic);
    }
} 