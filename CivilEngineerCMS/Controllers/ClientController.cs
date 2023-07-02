namespace CivilEngineerCMS.Web.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using ViewModels.Client;

    public class ClientController : BaseController
    {
        private readonly IClientService  clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }
        public async Task<IActionResult> Mine()
        {
            IEnumerable<MineClientManagerProjectViewModel> viewModel = await this.clientService.AllProjectsByUserIdAsync(this.User.GetId());
            return View(viewModel);
        }
    }
}

/*
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
 */
