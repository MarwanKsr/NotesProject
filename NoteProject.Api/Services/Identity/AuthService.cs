using Microsoft.AspNetCore.Identity;
using NoteProject.Api.Services.Identity.Models;
using NoteProject.Core.Domain.Identity;
using NoteProject.Service.Identity;

namespace NoteProject.Api.Services.Identity
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;

        public AuthService(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<(bool LoginSucceed, LoginResponseModel LoginResponseModel, bool isLocked, DateTime? lockUntil)> Login(LoginRequestModel loginRequestModel)
        {
            if (await _userManager.FindByEmailAsync(loginRequestModel.UserName) is not ApplicationUser user)
                return (false, new(), false, null);

            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
                return (false, new(), true, user.LockoutEnd.Value.LocalDateTime);

            var loginResult = await _signInManager.Authenticate(user.UserName, loginRequestModel.Password, true);


            if (!loginResult.Result.Succeeded && !loginResult.Result.IsNotAllowed)
                return (false, new(), true, null);

            await _userManager.ResetAccessFailedCountAsync(user);

            await _userManager.UpdateAsync(user);

            var rolenames = await _userManager.GetRolesAsync(user);
            var model = new LoginResponseModel()
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                AccessToken = loginResult.AccessToken,
                AccessTokenExpiry = loginResult.AccessTokenExpiry,
                Roles = rolenames
            };

            return (true, model, false, null);
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
    }
}
