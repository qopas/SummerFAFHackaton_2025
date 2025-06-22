using AutoMapper;
using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Entities;

namespace Ediki.Application.Mappings;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<Domain.Entities.Task, TaskDto>()
            .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.Assignee != null ? $"{src.Assignee.FirstName} {src.Assignee.LastName}" : null))
            .ForMember(dest => dest.AssigneeEmail, opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Email : null))
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}"));

        CreateMap<TaskDto, Domain.Entities.Task>()
            .ForMember(dest => dest.Assignee, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore())
            .ForMember(dest => dest.Sprint, opt => opt.Ignore());
    }
} 