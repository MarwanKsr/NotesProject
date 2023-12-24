using Microsoft.AspNetCore.Identity;
using NoteProject.Core.Base;
using NoteProject.Core.Domain.Identity;
using NoteProject.Host.Models;
using NoteProject.Service.Identity;
using NoteProject.Service.Identity.Models;

namespace NoteProject.Host.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IApplicationUserService _applicationUserService;

        public AccountService(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IApplicationUserService applicationUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationUserService = applicationUserService;
        }
        public async Task<(bool LoginSucceed, bool isLocked, DateTime? lockUntil)> Login(LoginRequestModel loginRequestModel)
        {
            if (await _userManager.FindByEmailAsync(loginRequestModel.UserName) is not ApplicationUser user)
                return (false, false, null);

            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
                return (false, true, user.LockoutEnd.Value.LocalDateTime);

            var loginResult = await _signInManager.Authenticate(user.UserName, loginRequestModel.Password, true);


            if (!loginResult.Result.Succeeded && !loginResult.Result.IsNotAllowed)
                return (false, true, null);

            await _userManager.ResetAccessFailedCountAsync(user);

            await _userManager.UpdateAsync(user);

            var rolenames = await _userManager.GetRolesAsync(user);

            return (true, false, null);
        }

        public async Task<IdentityResult> Register(ApplicationUser user, string password, string role)
        {
            if (await _userManager.IsEmailExistsAsync(user.Email))
                throw new ArgumentException("Email Already Exists");

            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return result;

            result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }

        public async Task<CommandResult<ResetPasswordResult>> SendResetPassword(string email)
        {
            return await _applicationUserService.SendResetPasswordByEmailAsync(email);
        }

        public async Task<CommandResult<ResetPasswordResult>> SendSetNewPassword(string email)
        {
            return await _applicationUserService.SendSetNewPasswordByEmailAsync(email);
        }

        public async Task<CommandResult<ResetPasswordResult>> SetNewPassword(string email, string password, string token)
        {
            return await _applicationUserService.ResetPasswordByEmailAsync(email, password, token);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<CommandResult<ResetPasswordResult>> ValidateSendResetPassword(string email, string token)
        {
            return await _applicationUserService.ValidateResetPasswordByEmailTokenAsync(email, token);
        }
    }
}
