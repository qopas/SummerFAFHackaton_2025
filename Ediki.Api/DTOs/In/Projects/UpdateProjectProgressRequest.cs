using Ediki.Api.DTOs.In;
using Ediki.Application.Features.Projects.Commands.UpdateProjectProgress;
using Ediki.Domain.Enums;

namespace SummerFAFHackaton_2025.DTOs.In.Projects;

public class UpdateProjectProgressRequest : IRequestIn<UpdateProjectProgressCommand>
{
    public string ProjectId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public UpdateProjectProgressRequest() { }

    public UpdateProjectProgressRequest(string projectId, string status)
    {
        ProjectId = projectId;
        Status = status;
    }

    public UpdateProjectProgressCommand Convert()
    {
        if (!Enum.TryParse<ProjectStatus>(Status, true, out var status))
        {
            throw new ArgumentException($"Invalid status value: {Status}. Valid values are: {string.Join(", ", Enum.GetNames<ProjectStatus>())}");
        }

        return new UpdateProjectProgressCommand(ProjectId, status);
    }
} 