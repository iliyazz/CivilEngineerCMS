using Microsoft.Extensions.Caching.Memory;

namespace CivilEngineerCMS.Web.Controllers
{
    using Data.Models;

    using Griesoft.AspNetCore.ReCaptcha;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.User;

    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMemoryCache memoryCache;

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMemoryCache memoryCache)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Returns the user registration form view.
        /// </summary>
        /// <returns>The registration form view.</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }
        /// <summary>
        /// Registers a new application user using the submitted form data.
        /// </summary>
        /// <param name="formModel">The form model containing the new user's email and password.</param>
        /// <returns>Redirects to the home page on success, or returns the registration form with errors on failure.</returns>
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
            this.memoryCache.Remove(OnLineClientsCacheKey);
            return this.RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Returns the login form view, clearing any existing external authentication session.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after a successful login. If null, defaults to the home page.</param>
        /// <returns>The login form view pre-populated with the return URL.</returns>
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
        /// <summary>
        /// Authenticates the user with the provided credentials and signs them in.
        /// </summary>
        /// <param name="formModel">The form model containing the user's email, password, and remember-me preference.</param>
        /// <returns>Redirects to the return URL (or home) on success, or returns the login form with an error message on failure.</returns>
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
                this.TempData[ErrorMessage] = this.L["There was an error while logging. Please contact again later or contact an administrator."].Value;
                this.memoryCache.Remove(OnLineClientsCacheKey);
                return this.View(formModel);

            }
            return this.LocalRedirect(formModel.ReturnUrl ?? "/Home/Index");

        }

    }
}
