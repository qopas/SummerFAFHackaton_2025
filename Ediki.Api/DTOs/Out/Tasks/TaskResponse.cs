using Ediki.Api.DTOs.Out;
using Ediki.Application.Features.Tasks.DTOs;

namespace Ediki.Api.DTOs.Out.Tasks;

public class TaskResponse : IResponseOut<TaskDto>
{
    public object? Convert(TaskDto result)
    {
        return result;
    }
} 