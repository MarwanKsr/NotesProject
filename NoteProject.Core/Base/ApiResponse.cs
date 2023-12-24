
namespace NoteProject.Core.Base;

public record ApiResponseData(string Status);
public class ApiResponse : ApiResponse<ApiResponseData> { }

public class ApiResponse<T>
{
    public const string GENERAL_KEY = "general";

    public T Data { get; private set; }

    public bool Status => !Errors.Any();

    public Dictionary<string, string> Errors { get; private set; }

    public string RequestId => Guid.NewGuid().ToString("D");

    public DateTime TimeStamp => DateTime.UtcNow;

    protected ApiResponse()
    {
        Errors = new Dictionary<string, string>();
    }

    protected ApiResponse(T data) : this()
    {
        Data = data;
    }

    protected ApiResponse(Dictionary<string, string> errors) : this()
    {
        Errors = errors ?? new();
    }

    /// <summary>
    /// Create Response Object
    /// </summary>
    /// <param name="data"></param>
    public static ApiResponse<T> Create(T data)
        => new(data);

    /// <summary>
    /// Create Failure object with single error message and general key
    /// </summary>
    /// <param name="msg"></param>
    public static ApiResponse<T> Failure(string msg)
        => new(new Dictionary<string, string>() { { GENERAL_KEY, msg } });
}
