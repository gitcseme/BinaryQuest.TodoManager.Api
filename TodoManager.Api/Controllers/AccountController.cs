using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoManager.Membership.AuthModels;
using TodoManager.Membership.Services;

namespace TodoManager.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILoggerManager _logger;

        public AccountController(IAccountService accountService, ILoggerManager logger)
        {
            _accountService = accountService;
            _logger = logger;
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
    }
}
