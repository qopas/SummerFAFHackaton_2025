using AutoMapper;
using Ediki.Application.Features.Sprints.DTOs;
using Ediki.Domain.Entities;

namespace Ediki.Application.Mappings;

public class SprintMappingProfile : Profile
{
    public SprintMappingProfile()
    {
        CreateMap<Sprint, SprintDto>()
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks != null ? src.Tasks.Count : 0))
            .ForMember(dest => dest.CompletedTaskCount, opt => opt.MapFrom(src => src.Tasks != null ? src.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Completed) : 0));

        CreateMap<SprintDto, Sprint>()
            .ForMember(dest => dest.Project, opt => opt.Ignore())
            .ForMember(dest => dest.Tasks, opt => opt.Ignore());
    }
} 