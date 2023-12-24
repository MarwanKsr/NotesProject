using Microsoft.AspNetCore.Identity;

namespace NoteProject.Service.Identity
{
    public interface IApplicationUserCommandsService
    {
        Task<IdentityResult> UpdatePasswordAsync(string userId, string newPassword);
    }
}
