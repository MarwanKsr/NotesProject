using System.Security.Claims;

namespace NoteProject.Core.Extensions;

public static class ClaimsPrincipalExtenstions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }
}
