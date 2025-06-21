using Ediki.Api.DTOs.Out;

namespace SummerFAFHackaton_2025.DTOs.Out.Teams;

public class BooleanResponse : IResponseOut<bool>
{
    public bool Success { get; set; }

    public BooleanResponse() { }

    public BooleanResponse(bool result)
    {
        Success = result;
    }

    public object? Convert(bool result)
    {
        return new BooleanResponse(result);
    }
}