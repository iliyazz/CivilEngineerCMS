namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using Services.Data.Models.Statistics;

    public class HomeController : BaseAdminController
    {
        private readonly IProjectService projectService;

        public HomeController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns the system statistics (projects, active projects, clients) as JSON for the admin dashboard.
        /// Served by the web app itself so the request is same-origin and uses the admin's login cookie.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            StatisticsServiceModel statistics = await this.projectService.GetStatisticsAsync();
            return this.Json(statistics);
        }
    }
}
