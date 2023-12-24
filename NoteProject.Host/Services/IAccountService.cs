using Microsoft.AspNetCore.Identity;
using NoteProject.Core.Base;
using NoteProject.Core.Domain.Identity;
using NoteProject.Host.Models;
using NoteProject.Service.Identity.Models;

namespace NoteProject.Host.Services
{
    public interface IAccountService
    {
        Task<(bool LoginSucceed, bool isLocked, DateTime? lockUntil)> Login(LoginRequestModel loginRequestModel);
        Task<IdentityResult> Register(ApplicationUser user, string password, string role);
        Task<CommandResult<ResetPasswordResult>> SendResetPassword(string email);
        Task<CommandResult<ResetPasswordResult>> ValidateSendResetPassword(string email, string token);
        Task<CommandResult<ResetPasswordResult>> SendSetNewPassword(string email);
        Task<CommandResult<ResetPasswordResult>> SetNewPassword(string email, string password, string token);
        Task SignOutAsync();
    }
}
