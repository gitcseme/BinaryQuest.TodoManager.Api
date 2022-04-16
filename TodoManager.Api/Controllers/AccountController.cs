using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoManager.Membership.AuthModels;
using TodoManager.Membership.Entities;
using TodoManager.Membership.Services;

namespace TodoManager.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILoggerManager _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            IAccountService accountService,
            ILoggerManager logger,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            try
            {
                var user = await _accountService.SignupAsync(model);
                _logger.LogInfo($"Successfull signup by {user.Email}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in signup: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Signin(SigninModel model)
        {
            try
            {
                var user = await _accountService.SigninAsync(model);
                _logger.LogInfo($"Successfull login by {user.Email}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in signin: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("signout")]
        public async Task<IActionResult> Signout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Signout exception");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
