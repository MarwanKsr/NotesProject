using Microsoft.AspNetCore.Identity;
using NoteProject.Core.Domain.Identity;

namespace NoteProject.Service.Identity.Models;

public record LoginResult(SignInResult Result, ApplicationUser User, string AccessToken,
    DateTime AccessTokenExpiry);
