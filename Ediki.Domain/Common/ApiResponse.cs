namespace Ediki.Domain.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string error)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Operation failed",
            Errors = new List<string> { error }
        };
    }

    public static ApiResponse<T> ErrorResponse(List<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Operation failed",
            Errors = errors
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResponse(string message = "Operation completed successfully")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }

    public new static ApiResponse ErrorResponse(string error)
    {
        return new ApiResponse
        {
            Success = false,
            Message = "Operation failed",
            Errors = new List<string> { error }
        };
    }

    public new static ApiResponse ErrorResponse(List<string> errors)
    {
        return new ApiResponse
        {
            Success = false,
            Message = "Operation failed",
            Errors = errors
        };
    }
}
