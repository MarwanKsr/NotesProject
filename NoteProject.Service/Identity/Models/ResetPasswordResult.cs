
namespace NoteProject.Service.Identity.Models;

public enum ResetPasswordResult
{
    Success,
    UserNotExists,
    TokenExpired,
    UserIsLocked,
    LockedUser
}
