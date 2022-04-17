using Newtonsoft.Json;

namespace TodoManager.Shared.CustomExceptions;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
