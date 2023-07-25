namespace CivilEngineerCMS.Web.Controllers
{
    using System.Security.Claims;
    using Data.Models;
    using Griesoft.AspNetCore.ReCaptcha;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.User;
    using static Common.NotificationMessagesConstants;

    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateRecaptcha(Action =nameof(Register), ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
        public async Task<IActionResult> Register(RegisterFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(formModel);
            }

            ApplicationUser user = new ApplicationUser
            {

            };

            await this.userManager.SetEmailAsync(user, formModel.Email);
            await this.userManager.SetUserNameAsync(user, formModel.Email);
            IdentityResult result = await this.userManager.CreateAsync(user, formModel.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
                return this.View(formModel);
            }

            await this.signInManager.SignInAsync(user, false);
            return this.RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel formModel = new LoginFormModel
            {
                ReturnUrl = returnUrl
            };
            return this.View(formModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(formModel);
            }

            var result = await this.signInManager.PasswordSignInAsync(formModel.Email, formModel.Password, formModel.RememberMe, false);
            if (!result.Succeeded)
            {
                this.TempData[ErrorMessage] = "There was an error while logging. Please contact again later or contact an administrator.";
                return this.View(formModel);

            }
            return this.Redirect(formModel.ReturnUrl ?? "/Home/Index");

        }

    }
}
