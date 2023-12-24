
namespace NoteProject.Core.Constants;

public record ToasterResult(
    ToasterResultType Type,
    string Title,
    string Message,
    dynamic Data = default)
{
    public static ToasterResult Info(string title, string message)
    {
        var response = new ToasterResult(ToasterResultType.Info, title, message);

        return response;
    }

    public static ToasterResult Success(string title, string message, dynamic data = null)
    {
        var response = new ToasterResult(ToasterResultType.Success, title, message, data);

        return response;
    }

    public static ToasterResult Warning(string title, string message)
    {
        var response = new ToasterResult(ToasterResultType.Warning, title, message);

        return response;
    }

    public static ToasterResult Error(string title, string message)
    {
        var response = new ToasterResult(ToasterResultType.Error, title, message);

        return response;
    }
}

public enum ToasterResultType
{
    Success = 0,
    Error = 1,
    Warning = 2,
    Info = 3
}
