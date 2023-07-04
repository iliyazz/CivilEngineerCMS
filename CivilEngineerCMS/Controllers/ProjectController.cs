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

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectFormModel formModel)
        {
            bool managerExists = await this.projectService.EmployeeExistsByUserIdAsync(formModel.ManagerId.ToString());

            if (!managerExists)
            {
                this.ModelState.AddModelError(nameof(formModel.ManagerId), "Manager does not exist.");
            }

            bool clientExists = await this.projectService.ClientExistsByUserIdAsync(formModel.ClientId.ToString());
            if (!clientExists)
            {
                this.ModelState.AddModelError(nameof(formModel.ClientId), "Client does not exist.");
            }

            bool statusExists = this.projectService.StatusExists(formModel.Status.ToString());
            if (!statusExists)
            {
                this.ModelState.AddModelError(nameof(formModel.Status), "Status does not exist.");
            }

            if (DateTime.UtcNow > formModel.ProjectEndDate)
            {
                this.ModelState.AddModelError(nameof(formModel.ProjectEndDate), "Project End Date cannot be before start date.");
            }
            if (!this.ModelState.IsValid)
            {
                formModel.Managers = await this.managerService.AllManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                return this.View(formModel);
            }

            try
            {
                await this.projectService.CreateProjectAsync(formModel);
                //@ViewData["Title"];
                //this.ViewBag.AddSuccessMessage($"Project {formModel.Name} added successfully.");
                return this.RedirectToAction("All");
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while adding the project. Please try again later or contact administrator!");
                formModel.Managers = await this.managerService.AllManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                return this.View(formModel);
            }

            
        }
    }
}
