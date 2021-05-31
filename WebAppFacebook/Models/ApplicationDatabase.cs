using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAppFacebook.Models
{
    public class ApplicationDatabase : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDatabase(DbContextOptions<ApplicationDatabase> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
