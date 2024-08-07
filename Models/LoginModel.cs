using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

         [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
