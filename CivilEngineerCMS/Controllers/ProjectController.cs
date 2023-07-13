﻿namespace CivilEngineerCMS.Web.Controllers
{
    using CivilEngineerCMS.Web.ViewModels.Employee;

    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Project;

    using static Common.NotificationMessagesConstants;

    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;
        private readonly IClientService clientService;
        private readonly IEmployeeService employeeService;

        public ProjectController(IProjectService projectService, IEmployeeService employeeService,
            IClientService clientService)
        {
            this.projectService = projectService;
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

        public async Task<IActionResult> Details()
        {
            var projectId = (string?)Url.ActionContext.RouteData.Values["id"];
            if (projectId == null)
            {
                this.TempData[ErrorMessage] = "Project does not exist.";
                this.ModelState.AddModelError(string.Empty, "Project does not exist.");
                return this.RedirectToAction("All", "Project");
            }

            if (!await projectService.ProjectExistsByIdAsync(projectId))
            {
                this.TempData[ErrorMessage] = "Project does not exist.";
                this.ModelState.AddModelError(string.Empty, "Project does not exist.");
                return this.RedirectToAction("All", "Project");
            }
            DetailsProjectViewModel viewModel = await this.projectService.DetailsByIdProjectAsync(projectId);
            return this.View(viewModel);
        }

        
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("All", "Project");
            }

            var project = await this.projectService.GetProjectForEditByIdAsync(id);

            var currentUserId = this.User.GetId();

            var projectManagerUserId = await this.employeeService.GetManagerIdByUserIdAsync(currentUserId);

            if (projectManagerUserId != project.ManagerId.ToString())
            {
                this.TempData[ErrorMessage] = "You should be manager of this project to delete it.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {
                ProjectPreDeleteViewModel viewModel = await this.projectService.GetProjectForPreDeleteByIdAsync(id);
                this.TempData[WarningMessage] = $"You are going to delete project {viewModel.Name}.";
                return this.View(viewModel);
            }
            catch (Exception e)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, ProjectPreDeleteViewModel formModel)
        {

            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("All", "Project");
            }

            var project = await this.projectService.GetProjectForEditByIdAsync(id);

            var currentUserId = this.User.GetId();

            var projectManagerUserId = await this.employeeService.GetManagerIdByUserIdAsync(currentUserId);

            if (projectManagerUserId != project.ManagerId.ToString())
            {
                this.TempData[ErrorMessage] = "You should be manager of this project to delete it.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {
                await this.projectService.DeleteProjectByIdAsync(id);

                this.TempData[WarningMessage] =
                    $"Project {formModel.Name} deleted successfully.";
                return this.RedirectToAction("All", "Project");
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "An error occurred while deleting the project. Please try again later or contact administrator!";
                return this.RedirectToAction("All", "Project");
            }
        }


        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }

    }
}
/*


        [HttpPost]
        public async Task<IActionResult> Delete(string id, EmployeePreDeleteViewModel formModel)
        {

            bool employeeExists = await this.employeeService.EmployeeExistsByIdAsync(id);
            if (!employeeExists)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                await this.employeeService.DeleteEmployeeByIdAsync(id);

                this.TempData[WarningMessage] =
                    $"Employee {formModel.FirstName} {formModel.LastName} deleted successfully.";
                return this.RedirectToAction("All", "Employee");
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] =
                    "An error occurred while deleting the employee. Please try again later or contact administrator!";
                return GeneralError();
            }
        }
 */