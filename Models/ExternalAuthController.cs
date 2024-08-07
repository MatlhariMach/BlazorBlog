using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace BlazorBlog.Models
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class ExternalAuthController : ControllerBase
        {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ExternalAuthController> _logger;

        public ExternalAuthController(SignInManager<ApplicationUser> signInManager, ILogger<ExternalAuthController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Ok(new { Success = true });
                }
                if (result.RequiresTwoFactor)
                {
                    return Ok(new { RequiresTwoFactor = true });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return Ok(new { IsLockedOut = true });
                }
                else
                {
                    return BadRequest(new { Error = "Invalid login attempt." });
                }
            }

            return BadRequest(new { Error = "Invalid login attempt." });
        }
    }
}
    

