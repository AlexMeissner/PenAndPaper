namespace DataTransfer
{
    //public enum ErrorCode
    //{
    //    Exception,
    //    Initialization,
    //    InvalidLogin,
    //    NoContent,
    //    Unauthorized,
    //    TokenAlreadyExists,
    //}
    //
    //public record ErrorDetails(ErrorCode Code, string Message)
    //{
    //    public string TraceIdentifier { get; set; } = string.Empty;
    //}
    //
    //public record HttpResponse(ErrorDetails? Error)
    //{
    //    public static HttpResponse Success => new((ErrorDetails?)null);
    //    public static HttpResponse Failure(ErrorDetails error)
    //    {
    //        return new HttpResponse(error);
    //    }
    //}
    //
    //public record HttpResponse<T>(T Data, ErrorDetails? Error) : HttpResponse(Error)
    //{
    //    public new static HttpResponse<T> Success(T data)
    //    {
    //        return new HttpResponse<T>(data, null);
    //    }
    //
    //    public new static HttpResponse<T> Failure(ErrorDetails error)
    //    {
    //        return new HttpResponse<T>(default!, error);
    //    }
    //}
}