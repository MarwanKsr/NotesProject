using Microsoft.AspNetCore.Identity;
using NoteProject.Core.Base;
using NoteProject.Core.Configuration;
using NoteProject.Core.Domain.Identity;
using NoteProject.Service.Identity.Models;
using NoteProject.Service.MessageSender;
using System.Text;

namespace NoteProject.Service.Identity;

public class ApplicationUserService : IApplicationUserService
{
    private readonly ApplicationUserManager _userManager;
    private readonly IApplicationUserCommandsService _applicationUserCommandsService;
    private readonly IEmailMessageSender _emailMessageSender;

    public ApplicationUserService(ApplicationUserManager userManager,
        IApplicationUserCommandsService applicationUserCommandsService,
        IEmailMessageSender emailMessageSender)
    {
        _userManager = userManager;
        _applicationUserCommandsService = applicationUserCommandsService;
        _emailMessageSender = emailMessageSender;
    }

    #region Send Reset Password

    public async Task<CommandResult<ResetPasswordResult>> SendResetPasswordByEmailAsync(string emailAddress)
    {
        var user = await _userManager.FindByEmailAsync(emailAddress);

        return await SendResetPasswordAsync(user);
    }

    private async Task<CommandResult<ResetPasswordResult>> SendResetPasswordAsync(ApplicationUser user)
    {
        if (user is null)
            return new(ResetPasswordResult.UserNotExists, false, "User not exists");

        var checkIsLocked = await _userManager.IsLockedOutAsync(user);
        if (checkIsLocked)
            return new(ResetPasswordResult.LockedUser, false, $"Your account is locked temporarily for security reasons until {user.LockoutEnd.Value}");

        await _userManager.UpdateSecurityStampAsync(user);

        var token = await _userManager.GenerateUserTokenAsync(
            user: user,
            tokenProvider: TokenOptions.DefaultEmailProvider,
            purpose: ApplicationUserManager.ResetPasswordTokenPurpose);

        var body = GetForgetPasswordEmailBody(user, token);
        await _emailMessageSender.SendMessageAsync(user.Email, "Reset Password", body);

        return new(ResetPasswordResult.Success);
    }

    private string GetForgetPasswordEmailBody(ApplicationUser user, string token)
    {
        var result = new StringBuilder();
        result.Append($"Hello {user.GetFullName()} ");
        result.AppendLine("Please click on this link to reset your password: ");
        result.Append($"{HostAppSetting.Instance.SiteUrl}/Account/ForgetPasswordHandler?email={user.Email}&token={token} ");
        result.AppendLine("If you did not request this, you can safely ignore this message");
        result.Append(" - ");
        result.Append("another user may have entered their email address incorrectly.");

        return result.ToString();
    }
    #endregion

    #region Validate Reset Password

    public async Task<CommandResult<ResetPasswordResult>> ValidateResetPasswordByEmailTokenAsync(string emailAddress, string token)
    {
        var user = await _userManager.FindByEmailAsync(emailAddress);

        return await ValidateResetPasswordTokenAsync(user, token);
    }

    private async Task<CommandResult<ResetPasswordResult>> ValidateResetPasswordTokenAsync(ApplicationUser user, string token)
    {
        if (user is null)
            return new(ResetPasswordResult.UserNotExists, false);

        var isValidToken = await _userManager.VerifyUserTokenAsync(
            user: user,
            tokenProvider: TokenOptions.DefaultEmailProvider,
            purpose: ApplicationUserManager.ResetPasswordTokenPurpose,
            token: token);

        if (!isValidToken)
            return new(ResetPasswordResult.TokenExpired, false);

        return new(ResetPasswordResult.Success);
    }

    #endregion

    #region Send Set New Password

    public async Task<CommandResult<ResetPasswordResult>> SendSetNewPasswordByEmailAsync(string emailAddress)
    {
        var user = await _userManager.FindByEmailAsync(emailAddress);

        return await SendSetNewPasswordAsync(user);
    }

    private async Task<CommandResult<ResetPasswordResult>> SendSetNewPasswordAsync(ApplicationUser user)
    {
        if (user is null)
            return new(ResetPasswordResult.UserNotExists, false);

        var checkIsLocked = await _userManager.IsLockedOutAsync(user);
        if (checkIsLocked)
            return new(ResetPasswordResult.LockedUser, false, user.LockoutEnd.Value.ToString());

        await _userManager.UpdateSecurityStampAsync(user);

        var token = await _userManager.GenerateUserTokenAsync(
            user: user,
            tokenProvider: TokenOptions.DefaultEmailProvider,
            purpose: ApplicationUserManager.ResetPasswordTokenPurpose);

        return new(ResetPasswordResult.Success);
    }

    #endregion

    #region Reset Password 

    public async Task<CommandResult<ResetPasswordResult>> ResetPasswordByEmailAsync(string emailAddress, string newPassword, string token)
    {
        var user = await _userManager.FindByEmailAsync(emailAddress);

        return await ResetPasswordAsync(user, newPassword, token);
    }

    private async Task<CommandResult<ResetPasswordResult>> ResetPasswordAsync(ApplicationUser user, string newPassword, string token)
    {
        if (user is null)
            return new(ResetPasswordResult.UserNotExists, false, "User not exsists");

        var (verficationResult, _, _) = await ValidateResetPasswordTokenAsync(user, token);
        if (verficationResult is ResetPasswordResult.TokenExpired)
            await _userManager.AccessFailedAsync(user); //Increment fail count

        if (verficationResult is not ResetPasswordResult.Success)
            return new(verficationResult, false);

        var resetPasswordResult = await _applicationUserCommandsService.UpdatePasswordAsync(user.Id, newPassword);
        if (!resetPasswordResult.Succeeded)
            return new(ResetPasswordResult.TokenExpired, false, "Token is expired");

        return new(ResetPasswordResult.Success);
    }

    #endregion
}
