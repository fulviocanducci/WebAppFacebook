using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAppFacebook.Models;

namespace WebAppFacebook.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<ApplicationRole> RoleManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public LoginController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser result = await UserManager.FindByEmailAsync("fulviocanducci@hotmail.com");
            if (result == null)
            {
                result = new ApplicationUser
                {
                    Email = "fulviocanducci@hotmail.com",
                    UserName = "fulviocanducci@hotmail.com",
                    Name = "Fúlvio"
                };
                await UserManager.CreateAsync(result, "Ab770301@");
            }
            return View();
        }

        public async Task<IActionResult> Auth(Login login)
        {
            var result = await SignInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect("~/");
            }
            return View();
        }
    }
}
