using Ediki.Application.Features.Projects.Commands.DeleteProject;

namespace Ediki.Api.DTOs.In.Projects;

public class DeleteProjectRequest : IRequestIn<DeleteProjectCommand>
{
    public string ProjectId { get; }

    public DeleteProjectRequest(string projectId)
    {
        ProjectId = projectId;
    }

    public DeleteProjectCommand Convert()
    {
        return new DeleteProjectCommand(ProjectId);
    }
} 