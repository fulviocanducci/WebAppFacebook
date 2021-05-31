using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppFacebook.Models;

namespace WebAppFacebook.Controllers
{
    public class FacebookController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<ApplicationRole> RoleManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public FacebookController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }
        public IActionResult Index()
        {
            var properties = SignInManager
                .ConfigureExternalAuthenticationProperties(FacebookDefaults.AuthenticationScheme, Url.Action("Callback", "Facebook"));
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> Callback()
        {
            ExternalLoginInfo info = await SignInManager.GetExternalLoginInfoAsync();

            Microsoft.AspNetCore.Identity.SignInResult resultExternalLogin =
                await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (!resultExternalLogin.Succeeded)
            {
                var nameIdentifier = info.Principal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var email = info.Principal.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
                var name = info.Principal.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();
                ApplicationUser applicationUser = new()
                {
                    Email = email.Value,
                    UserName = email.Value,
                    Name = name.Value
                };
                IdentityResult identityResultUser = await UserManager.CreateAsync(applicationUser, $"fB@{nameIdentifier.Value}#");
                if (identityResultUser.Succeeded)
                {
                    IdentityResult identityResultLogin = await UserManager.AddLoginAsync(applicationUser, info);
                    if (identityResultLogin.Succeeded)
                    {
                        await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
                    }
                }
            }

            return Redirect("~/");
        }
    }
}
