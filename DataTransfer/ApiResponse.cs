namespace DataTransfer
{
    public enum ErrorCode
    {
        Exception,
        Initialization,
        InvalidLogin,
        NoContent,
        Unauthorized,
        TokenAlreadyExists,
    }

    public record ErrorDetails(ErrorCode Code, string Message)
    {
        public string TraceIdentifier { get; set; } = string.Empty;
    }

    public record ApiResponse(ErrorDetails? Error)
    {
        public static ApiResponse Success => new((ErrorDetails?)null);
        public static ApiResponse Failure(ErrorDetails error)
        {
            return new ApiResponse(error);
        }
    }

    public record ApiResponse<T>(T Data, ErrorDetails? Error) : ApiResponse(Error)
    {
        public new static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>(data, null);
        }

        public new static ApiResponse<T> Failure(ErrorDetails error)
        {
            return new ApiResponse<T>(default!, error);
        }
    }
}