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

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            try
            {
                var user = await _accountService.SignupAsync(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
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
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
