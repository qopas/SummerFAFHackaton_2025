using Ediki.Api.DTOs.Out;
using Ediki.Application.Features.Tasks.DTOs;

namespace Ediki.Api.DTOs.Out.Tasks;

public class TaskResponse : IResponseOut<TaskDto>
{
    public TaskDto? Data { get; set; }

    public TaskResponse() { }

    public TaskResponse(TaskDto task)
    {
        Data = task;
    }

    public object? Convert(TaskDto result)
    {
        return new TaskResponse(result);
    }
} 