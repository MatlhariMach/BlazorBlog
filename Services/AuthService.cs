namespace BlazorBlog.Services
{
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using BlazorBlog.Models;

    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel registerModel)
        {
            var user = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Address = registerModel.Address,
                PhoneNumber = registerModel.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            return result;
        }

        public async Task<bool> SignInUserAsync(ApplicationUser user)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }
    }


}
