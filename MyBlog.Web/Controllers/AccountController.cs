using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Web.Models.ViewModels;

namespace MyBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        // Create Post method for Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
            };

            // store the password. for privacy reasons
            var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
            if (identityResult.Succeeded)
            {
                // Assign this user the "User" role (newly created user)
                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                if (roleIdentityResult.Succeeded)
                {
                    // Show success notification
                    return RedirectToAction("Register");
                }

            }
            // Show error notification
            return View();
        }
    }
}
