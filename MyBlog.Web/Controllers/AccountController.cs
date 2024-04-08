using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Web.Models.ViewModels;

namespace MyBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            // CreateAsync come from IdentityUser
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

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            // Create ReturnUrl to redirect user to the page they were trying to access
            // Ex: https://localhost:7210/AdminTags/Add -> /AdminTags/Add
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl,
            };
            return View(model);   
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            // check if the input password is the same as the password in Db
            if (loginViewModel.UserName != null && loginViewModel.Password != null)
            {
                var signInResult = await signInManager.PasswordSignInAsync(
                    loginViewModel.UserName, loginViewModel.Password, false, false);


                if (signInResult != null && signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }

                    // Show success notification
                    return RedirectToAction("Index", "Home");
                }
            }

            // Show error notification
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Applied Logout fuction from SignInManager
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
