namespace CivilEngineerCMS.Web.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Project;

    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;
        private readonly IManagerService managerService;
        private readonly IClientService clientService;

        public ProjectController(IProjectService projectService, IManagerService managerService, IClientService clientService)
        {
            this.projectService = projectService;
            this.managerService = managerService;
            this.clientService = clientService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<AllProjectViewModel> viewModel = await this.projectService.AllProjectsAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddProjectFormModel formModel = new AddProjectFormModel
            {
                Managers = await this.managerService.AllManagersAsync(),
                Clients = await this.clientService.AllClientsAsync()
            };
                
            return this.View(formModel);
        }
    }
}
