using Microsoft.AspNetCore.Identity;

namespace WebAppFacebook.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Name { get; set; }
    }
}
