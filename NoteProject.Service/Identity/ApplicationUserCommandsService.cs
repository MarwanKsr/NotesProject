using Microsoft.AspNetCore.Identity;
using NoteProject.Core.Domain.Identity;

namespace NoteProject.Service.Identity
{
    public class ApplicationUserCommandsService : IApplicationUserCommandsService
    {
        private readonly ApplicationUserManager _applicationUserManager;

        public ApplicationUserCommandsService(ApplicationUserManager applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        public async Task<IdentityResult> UpdatePasswordAsync(string userId, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                var error = new IdentityError() { Description = "Invalid Password Syntax" };
                return IdentityResult.Failed(error);
            }

            var user = await _applicationUserManager.FindByIdAsync(userId);

            user.PasswordHash = HashPassword(user, newPassword);

            var result = await _applicationUserManager.UpdateAsync(user);

            return result;
        }

        public static string HashPassword(ApplicationUser user, string password)
        {
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            return passwordHasher.HashPassword(user, password);
        }
    }
}
