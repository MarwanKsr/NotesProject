using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteProject.Core.Domain.Identity;
using NoteProject.Core.Roles;
using NoteProject.Host.Base.Controllers;
using NoteProject.Host.Models;
using NoteProject.Host.Services;

namespace NoteProject.Host.Controllers
{
    public class AccountController : BaseMvcControllers
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("[Controller]/Login")]
        public ActionResult Login()
        {
            var loginModel = new LoginRequestModel();
            return View(loginModel);
        }

        [HttpPost("[Controller]/Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequestModel loginRequestModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                var message = string.Join("\n", errors);
                return ErrorJsonResult("Login", message);
            };

            var (loginSucceed, isLocked, lockedUntil) = await _accountService.Login(loginRequestModel);

            if (isLocked && lockedUntil == null)
            {
                return ErrorJsonResult("Login", "User wrong credentials");
            }

            if (isLocked)
            {
                return ErrorJsonResult("Login" ,$"User account is locked until {lockedUntil}");
            }

            if (!loginSucceed)
            {
                return ErrorJsonResult("Login", "User not exists");
            }
            return SuccessJsonResult("Login", "Login is successed");
        }

        [HttpGet("[Controller]/Register")]
        public ActionResult Register()
        {
            var registrationModel = new RegistrationModel();
            return View(registrationModel);
        }

        [HttpPost("[Controller]/Register")]
        public async Task<ActionResult> Register(RegistrationModel registrationModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return ErrorJsonResult("Register", message);
            }

            try
            {
                var user = new ApplicationUser(registrationModel.FirstName, registrationModel.LastName, registrationModel.Email);
                var identityResult = await _accountService.Register(user, registrationModel.Password, Roles.User);
                if (!identityResult.Succeeded)
                {
                    var message = string.Join("\n", identityResult.Errors.Select(e => e.Description));
                    return ErrorJsonResult("Register", message);
                }
            }
            catch (Exception ex)
            {
                return ErrorJsonResult("Register", ex.Message);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("[Controller]/ForgetPassword")]
        public ActionResult ForgetPassword()
        {
            var model = new ForgetPasswordModel();
            return View(model);
        }

        [HttpPost("[Controller]/ForgetPassword")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return ErrorJsonResult("Forget Password", message);
            }

            var (isSuccess, errorMessage) = await _accountService.SendResetPassword(model.Email);
            if (!isSuccess)
                return ErrorJsonResult("Forget Password", errorMessage);

            return SuccessJsonResult("Forget Password", "Reset password link was sent to your email");
        }

        [HttpGet("[Controller]/ForgetPasswordHandler")]
        public async Task<ActionResult> ForgetPasswordHandler([FromQuery] string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return ErrorJsonResult("Forget Password", "There is something got wrong");
            }

            var (isSuccess, errorMessage) = await _accountService.ValidateSendResetPassword(email, token);
            if (!isSuccess)
                return ErrorJsonResult("Forget Password", errorMessage);

            return Redirect($"/Account/SetNewPassword?email={email}&token={token}");
        }

        [HttpGet("[Controller]/SetNewPassword")]
        public ActionResult SetNewPassword([FromQuery] string email, string token)
        {
            var model = new SetNewPasswordModel()
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        public async Task<ActionResult> SetNewPassword(SetNewPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return ErrorJsonResult("Set New Password", message);
            }

            var (isSuccess, errorMessage) = await _accountService.SetNewPassword(model.Email, model.Password,model.Token);
            if (!isSuccess)
                return ErrorJsonResult("Set New Password", errorMessage);

            return SuccessJsonResult("Set New Pasword", "Password is changed successfully");
        }

        [HttpGet]
        [Authorize]
        [Route("[Controller]/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
