namespace CivilEngineerCMS.Web.Controllers
{
    using Data.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Home;
    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService, UserManager<ApplicationUser> userManager)
        {
            this.homeService = homeService;
        }
        /// <summary>
        /// Returns the home index view with the latest projects. Redirects administrators to the admin area.
        /// </summary>
        /// <returns>The index view with a collection of recent projects, or a redirect to the admin home page for administrators.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if(this.User.IsInRole(AdministratorRoleName))
            {
                return this.RedirectToAction("Index", "Home", new {Area = AdminAreaName});
            }
            IEnumerable<IndexViewModel> viewModel = await this.homeService.AllIndexProjectsAsync();

            return View(viewModel);
        }
        /// <summary>
        /// Returns the privacy policy page view.
        /// </summary>
        /// <returns>The privacy view.</returns>
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Returns the appropriate error view based on the HTTP status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code received (e.g. 400, 401, 404).</param>
        /// <returns>A specific error view for known status codes (400, 401, 404), or the generic error view for all others.</returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400 || statusCode == 404)
            {
                return this.View("Error404");
            }

            if (statusCode == 401)
            {
                return this.View("Error401");
            }

            return this.View();
        }
    }
}