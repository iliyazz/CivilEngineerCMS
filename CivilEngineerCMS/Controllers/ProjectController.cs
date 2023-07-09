namespace CivilEngineerCMS.Web.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis;

    using Services.Data.Interfaces;

    using ViewModels.Project;
    using static Common.NotificationMessagesConstants;

    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;
        //private readonly IManagerService managerService;
        private readonly IClientService clientService;
        private readonly IEmployeeService employeeService;

        public ProjectController(IProjectService projectService, IEmployeeService employeeService,
            IClientService clientService)
        {
            this.projectService = projectService;
            //this.managerService = managerService;
            this.clientService = clientService;
            this.employeeService = employeeService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<AllProjectViewModel> viewModel = await this.projectService.AllProjectsAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddAndEditProjectFormModel formModel = new AddAndEditProjectFormModel
            {
                Managers = await this.employeeService.AllManagersAsync(),
                //Managers = await this.managerService.AllManagersAsync(),
                Clients = await this.clientService.AllClientsAsync()
            };

            return this.View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAndEditProjectFormModel formModel)
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
                this.ModelState.AddModelError(nameof(formModel.Status), "Name does not exist.");
            }

            if (DateTime.UtcNow > DateTime.Parse(formModel.ProjectEndDate))
            {
                this.ModelState.AddModelError(nameof(formModel.ProjectEndDate),
                    "Project End Date cannot be before start date.");
            }

            if (!this.ModelState.IsValid)
            {
                formModel.Managers = await this.employeeService.AllManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                return this.View(formModel);
            }

            try
            {
                await this.projectService.CreateProjectAsync(formModel);

                this.TempData[SuccessMessage] = $"Project {formModel.Name} added successfully.";
                return this.RedirectToAction("All");
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the project. Please try again later or contact administrator!");
                formModel.Managers = await this.employeeService.AllManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                return this.View(formModel);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }

            var project = await this.projectService.GetProjectForEditByIdAsync(id);

            var currentUserId = this.User.GetId();
            //var currentManagerId = project.ManagerId.ToString();

            var projectManagerUserId = await this.employeeService.GetManagerIdByUserIdAsync(currentUserId);



            if (project.ManagerId.ToString() != projectManagerUserId)
            {
                this.TempData[ErrorMessage] = "You must be manager of project you want to edit.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {
                AddAndEditProjectFormModel formModel = await this.projectService.GetProjectForEditByIdAsync(id);
                formModel.Managers = await this.employeeService.AllManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();

                return this.View(formModel);
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "An error occurred while editing the project. Please try again later or contact administrator!";
                return this.RedirectToAction("Mine", "Employee");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddAndEditProjectFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                formModel.Managers = await this.employeeService.AllManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                return this.View(formModel);
            }

            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";

                return this.RedirectToAction("Mine", "Employee");
            }

            var project = await this.projectService.GetProjectForEditByIdAsync(id);


            var currentUserId = this.User.GetId();

            var projectManagerUserId = await this.employeeService.GetManagerIdByUserIdAsync(currentUserId);
            


            if (project.ManagerId.ToString() != projectManagerUserId)
            {
                this.TempData[ErrorMessage] = "You must be manager of project you want to edit.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {
                await this.projectService.EditProjectByIdAsync( id, formModel);

            }
            catch (Exception e)
            {
                this.TempData[ErrorMessage] = "An error occurred while editing the project. Please try again later or contact administrator!";
                return this.View(formModel);

            }
            this.TempData[SuccessMessage] = $"Project {formModel.Name} edited successfully.";
            formModel.Managers = await this.employeeService.AllManagersAsync();
            formModel.Clients = await this.clientService.AllClientsAsync();
            return this.RedirectToAction("Mine", "Employee");
        }
    }
}