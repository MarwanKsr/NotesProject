using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteProject.Api.Services.Identity;
using NoteProject.Api.Services.Identity.Models;
using NoteProject.Core.Base;
using NoteProject.Core.Domain.Identity;
using NoteProject.Core.Roles;
using NoteProject.Service.Identity;
using NoteProject.Service.Identity.Models;

namespace NoteProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IApplicationUserService _userService;

        public AuthController(IAuthService authService, IApplicationUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequestModel loginRequestModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                var message = string.Join("\n", errors);
                return BadRequest(ApiResponse<string>.Failure(message));
            };

            var (loginSucceed, loginResponse, isLocked, lockedUntil) = await _authService.Login(loginRequestModel);

            if (isLocked && lockedUntil == null)
            {
                return BadRequest(ApiResponse<string>.Failure("User wrong credentials"));
            }

            if (isLocked)
            {
                return BadRequest(ApiResponse<string>.Failure($"User account is locked until {lockedUntil}"));
            }

            if (!loginSucceed)
            {
                return BadRequest(ApiResponse<string>.Failure("User not exists"));
            }
            return Ok(ApiResponse<LoginResponseModel>.Create(loginResponse));
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegistrationModel registrationModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                var message = string.Join("\n", errors);
                return BadRequest(ApiResponse<string>.Failure(message));
            }

            try
            {
                var user = new ApplicationUser(registrationModel.FirstName, registrationModel.LastName, registrationModel.Email);
                var identityResult = await _authService.Register(user, registrationModel.Password, Roles.User);
                if (!identityResult.Succeeded)
                {
                    var message = string.Join("\n", identityResult.Errors.Select(e => e.Description));
                    return BadRequest(ApiResponse<string>.Failure(message));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Failure(ex.Message));
            }
            return Ok(ApiResponse<string>.Create("Registration completed successfully"));
        }

        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ForgetPassword(ForgetPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return BadRequest();

            var (verficationResult, _, errorMessage) = await _userService.SendResetPasswordByEmailAsync(model.Email);

            return verficationResult switch
            {
                ResetPasswordResult.Success => Ok(ApiResponse<string>.Create("Reset password sent Successfuly")),
                _ => BadRequest(ApiResponse<string>.Failure(errorMessage))
            };
        }
    }
}
