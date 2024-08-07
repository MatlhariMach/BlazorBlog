using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BlazorBlog.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;

namespace BlazorBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(SignInManager<ApplicationUser> signInManager, ILogger<LoginController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Ok();
            }
            if (result.RequiresTwoFactor)
            {
                return StatusCode(403, "RequiresTwoFactor");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return StatusCode(423, "IsLockedOut");
            }
            else
            {
                return Unauthorized("Invalid login attempt.");
            }
        }
    }
}
