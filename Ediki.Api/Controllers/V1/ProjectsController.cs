using Ediki.Api.DTOs;
using Ediki.Api.DTOs.In.Projects;
using Ediki.Application.Features.Projects.Commands.CreateProject;
using Ediki.Application.Features.Projects.Commands.DeleteProject;
using Ediki.Application.Features.Projects.Commands.ToggleProjectFeature;
using Ediki.Application.Features.Projects.Commands.UpdateProjectProgress;
using Ediki.Application.Features.Projects.Commands.UpdateProjectVisibility;
using Ediki.Application.Features.Projects.DTOs;
using Ediki.Application.Features.Projects.Queries.GetProjectById;
using Ediki.Application.Features.Projects.Queries.GetProjects;
using Ediki.Application.Features.ProjectMembers.Commands.InviteUserToProject;
using Ediki.Application.Features.ProjectMembers.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SummerFAFHackaton_2025.DTOs.In.Projects;
using SummerFAFHackaton_2025.DTOs.Out.Projects;
using Ediki.Domain.Common;

namespace SummerFAFHackaton_2025.Controllers.V1;

[Route("api/v1/[controller]")]
public class ProjectsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjects([FromQuery] GetProjectsRequest request)
    {
        return await ExecuteListQueryAsync<ProjectResponse, ProjectDto>(request.Convert());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectById(string id)
    {
        return await ExecuteQueryAsync<ProjectResponse, ProjectDto>(new GetProjectByIdQuery(id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        var command = request.Convert();
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            var baseres = new BaseResponse<object?>
            {
                Success = false,
                Message = "Operation failed",
                Errors = result.Errors.Any() ? result.Errors : new List<string> { result.Error ?? "Unknown error" }
            };
            return BadRequest(baseres);
        }

        var responseDto = new ProjectResponse();
        var response = new BaseResponse<object?>
        {
            Success = true,
            Data = responseDto.Convert(result.Value),
            Message = "Operation completed successfully"
        };

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("{id}/invite")]
    [ProducesResponseType(typeof(ProjectMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> InviteUserToProject(string id, [FromBody] InviteUserToProjectCommand command)
    {
        try
        {
            if (id != command.ProjectId)
                return BadRequest(new { 
                    success = false,
                    message = "Project ID mismatch",
                    errors = new[] { "Project ID in URL doesn't match request body" },
                    timestamp = DateTime.UtcNow
                });

            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new { 
                    success = false,
                    message = result.Error, 
                    errors = result.Errors,
                    timestamp = DateTime.UtcNow
                });
            }

            return Ok(new {
                success = true,
                data = result.Value,
                message = "User invited to project successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Error inviting user to project",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProject(string id)
    {
        await ExecuteAsync<DeleteProjectCommand, ProjectResponse, ProjectDto>(new DeleteProjectRequest(id));
        return NoContent();
    }

    [HttpPut("{id}/progress")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProjectProgress(string id, [FromBody] UpdateProjectProgressRequest request)
    {
        if (id != request.ProjectId)
            return BadRequest("ID mismatch");
            
        return await ExecuteAsync<UpdateProjectProgressCommand, ProjectResponse, ProjectDto>(request);
    }

    [HttpPut("{id}/visibility")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProjectVisibility(string id, [FromBody] UpdateProjectVisibilityRequest request)
    {
        if (id != request.ProjectId)
            return BadRequest("ID mismatch");
            
        return await ExecuteAsync<UpdateProjectVisibilityCommand, ProjectResponse, ProjectDto>(request);
    }
} 