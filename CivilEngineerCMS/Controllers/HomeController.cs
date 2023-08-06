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
        /// This method return index view with 5 projects for index page
        /// </summary>
        /// <returns></returns>
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
        /// This method return view for privacy page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// This method return view for error page
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
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