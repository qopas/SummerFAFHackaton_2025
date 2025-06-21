using Ediki.Api.DTOs.Out;
using Ediki.Application.Features.Sprints.DTOs;

namespace Ediki.Api.DTOs.Out.Sprints;

public class SprintResponse : IResponseOut<SprintDto>
{
    public SprintDto? Data { get; set; }

    public SprintResponse() { }

    public SprintResponse(SprintDto sprint)
    {
        Data = sprint;
    }

    public object? Convert(SprintDto result)
    {
        return new SprintResponse(result);
    }
} 