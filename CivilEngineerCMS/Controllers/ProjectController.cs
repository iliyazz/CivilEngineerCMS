namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using ViewModels.Project;

    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<AllProjectViewModel> viewModel = await this.projectService.AllProjectsAsync();
            return View(viewModel);
        }
    }
}
