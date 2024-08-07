using Microsoft.AspNetCore.Identity;

namespace BlazorBlog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
