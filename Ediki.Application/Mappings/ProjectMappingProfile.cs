using AutoMapper;
using Ediki.Application.Features.Projects.DTOs;
using Ediki.Domain.Entities;

namespace Ediki.Application.Mappings;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectDto, Project>();
    }
} 