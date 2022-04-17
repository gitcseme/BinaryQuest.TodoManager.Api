namespace TodoManager.Shared.CustomExceptions;

public class ApiException : Exception
{
    public ApiException(string Message, int StatusCode) : base(Message)
    {
        this.StatusCode = StatusCode;
    }

    public int StatusCode { get; set; }
}