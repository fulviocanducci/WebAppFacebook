using Microsoft.AspNetCore.Authentication.Facebook;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Facebook()
        {
            var properties = SignInManager
                .ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback", "Login"));
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookCallback()
        {
            ExternalLoginInfo info = await SignInManager.GetExternalLoginInfoAsync();
            //info.Principal //the IPrincipal with the claims from facebook
            //info.ProviderKey //an unique identifier from Facebook for the user that just signed in
            //info.LoginProvider //a string with the external login provider name, in this case Facebook

            //to sign the user in if there's a local account associated to the login provider
            var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);            
            if (!result.Succeeded)
            {
                ApplicationUser applicationUser = new()
                {
                    //applicationUser.Email = info.AuthenticationProperties
                };
                await UserManager.AddLoginAsync(applicationUser, info);        
            }

            return Redirect("~/");
        }
    }
}
