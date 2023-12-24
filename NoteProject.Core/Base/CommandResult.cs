
namespace NoteProject.Core.Base;

public record CommandResult(bool IsSuccess = true, string ErrorMessage = "");

public record CommandResult<T>(T Result, bool IsSuccess = true, string ErrorMessage = "") : CommandResult(IsSuccess, ErrorMessage);
