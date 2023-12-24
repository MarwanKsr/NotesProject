using Microsoft.AspNetCore.Identity;
using NoteProject.Api.Services.Identity.Models;
using NoteProject.Core.Domain.Identity;

namespace NoteProject.Api.Services.Identity
{
    public interface IAuthService
    {
        Task<(bool LoginSucceed, LoginResponseModel LoginResponseModel, bool isLocked, DateTime? lockUntil)> Login(LoginRequestModel loginRequestModel);
        Task<IdentityResult> Register(ApplicationUser user, string password, string role);
    }
}
