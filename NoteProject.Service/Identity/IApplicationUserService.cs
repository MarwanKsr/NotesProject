using NoteProject.Core.Base;
using NoteProject.Service.Identity.Models;

namespace NoteProject.Service.Identity;

public interface IApplicationUserService
{
    Task<CommandResult<ResetPasswordResult>> SendResetPasswordByEmailAsync(string emailAddress);
    Task<CommandResult<ResetPasswordResult>> ValidateResetPasswordByEmailTokenAsync(string emailAddress, string token);
    Task<CommandResult<ResetPasswordResult>> SendSetNewPasswordByEmailAsync(string emailAddress);
    Task<CommandResult<ResetPasswordResult>> ResetPasswordByEmailAsync(string emailAddress, string newPassword, string token);
}
