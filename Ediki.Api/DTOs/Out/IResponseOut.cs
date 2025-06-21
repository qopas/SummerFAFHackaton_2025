namespace Ediki.Api.DTOs.Out;

public interface IResponseOut<in T>
{
    object? Convert(T result);
}
