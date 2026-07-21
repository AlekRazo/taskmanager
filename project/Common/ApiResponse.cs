namespace TaskManager.Common;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    // Helper for Successful Responses
    public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Success = true,
            Message = message,
            Data = data
        };
    }

    // Helper for Error Responses
    public static ApiResponse<T> ErrorResponse(List<string> errors, string message = "An error occurred", int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
