using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoteProject.Core.Domain.Identity;

namespace NoteProject.Service.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<bool> IsUserNameExistsAsync(string username)
        {
            var query = Users.Where(x => x.UserName != null && x.UserName.Trim() == username.Trim());

            return await query.AnyAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var query = Users.Where(x => x.Email != null && x.Email.Equals(email));

            return await query.AnyAsync();
        }
    }
}
