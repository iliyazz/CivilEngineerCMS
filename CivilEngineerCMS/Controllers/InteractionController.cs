namespace CivilEngineerCMS.Web.Controllers
{
    using CivilEngineerCMS.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Interaction;

    using static Common.NotificationMessagesConstants;

    public class InteractionController : BaseController
    {
        private readonly IInteractionService interactionService;
        private readonly IEmployeeService employeeService;
        private readonly IClientService clientService;
        private readonly IProjectService projectService;

        public InteractionController(IInteractionService interactionService, IEmployeeService employeeService,
            IClientService clientService, IProjectService projectService)
        {
            this.interactionService = interactionService;
            this.employeeService = employeeService;
            this.clientService = clientService;
            this.projectService = projectService;
        }


        [HttpGet]
        public async Task<IActionResult> All(string id)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientAsync(userId);
            bool isAdministrator = this.User.IsAdministrator();

            bool isClientOfProject = false;
            if (isClient)
            {
                string clientIdByUserId = await this.clientService.GetClientIdByUserIdAsync(userId);
                string clientIdByProjectId = await this.clientService.GetClientIdByProjectIdAsync(id);
                isClientOfProject = string.Equals(clientIdByUserId, clientIdByProjectId,
                    StringComparison.CurrentCultureIgnoreCase);
            }

            bool isManagerOfProject = false;
            bool isEmployeeOfProject = false;
            if (isEmployee)
            {
                string employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
                isEmployeeOfProject = await this.projectService.IsEmployeeOfProjectAsync(id, employeeId);
            }

            if (!(isEmployeeOfProject || isManagerOfProject || isClientOfProject || isAdministrator))
            {
                this.TempData[ErrorMessage] = "You are not authorized to view interaction to this project.";
                return RedirectToAction("Index", "home");
            }

            if ((isEmployeeOfProject || isManagerOfProject || isAdministrator) &&
                !await this.interactionService.InteractionExistsByProjectIdAsync(id))
            {
                this.TempData[InfoMessage] = "There are no interaction about this project. Create the first one.";
                return RedirectToAction("Add", "Interaction", new { id = id });
            }

            if (isClientOfProject && !await this.interactionService.InteractionExistsByProjectIdAsync(id))
            {
                this.TempData[InfoMessage] = "There are no interaction about this project.";
                return RedirectToAction("Mine", "Client", new { id = id });
            }

            IEnumerable<AddAndEditInteractionFormModel> viewModel =
                await this.interactionService.AllInteractionsByProjectIdAsync(id);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add(string id)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientAsync(userId);
            bool isAdministrator = this.User.IsAdministrator();


            if (isClient)
            {
                this.TempData[ErrorMessage] = "You are not authorized to add interaction to this project.";
                return RedirectToAction("Mine", "Client");
            }

            bool isManagerOfProject = false;
            bool isEmployeeOfProject = false;
            if (isEmployee)
            {
                string employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
                isEmployeeOfProject = await this.projectService.IsEmployeeOfProjectAsync(id, employeeId);
            }

            if (!(isEmployeeOfProject || isManagerOfProject || isAdministrator))
            {
                this.TempData[ErrorMessage] = "You are not authorized to add interaction to this project.";
                return RedirectToAction("Mine", "Employee");
            }


            AddAndEditInteractionFormModel formModel = new AddAndEditInteractionFormModel
            {
                ProjectId = Guid.Parse(id),
                Date = DateTime.UtcNow,
            };
            return View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string id, AddAndEditInteractionFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientAsync(userId);
            bool isAdministrator = this.User.IsAdministrator();

            if (isClient)
            {
                this.TempData[ErrorMessage] = "You are not authorized to add interaction to this project.";
                return RedirectToAction("Mine", "Client");
            }

            bool isManagerOfProject = false;
            bool isEmployeeOfProject = false;
            if (isEmployee)
            {
                string employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
                isEmployeeOfProject = await this.projectService.IsEmployeeOfProjectAsync(id, employeeId);
            }

            if (!(isEmployeeOfProject || isManagerOfProject || isAdministrator))
            {
                this.TempData[ErrorMessage] = "You are not authorized to add interaction to this project.";
                return RedirectToAction("Mine", "Employee");
            }

            try
            {
                await this.interactionService.CreateInteractionAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Interaction added successfully.";
                return this.RedirectToAction("All", "Interaction", new { id = id });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the interaction. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string projectId, string interactionId)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientAsync(userId);
            bool isAdministrator = this.User.IsAdministrator();

            if (isClient)
            {
                this.TempData[ErrorMessage] = "You are not authorized to edit interaction to this project.";
                return RedirectToAction("Mine", "Client");
            }

            bool isManagerOfProject = false;
            if (isEmployee)
            {
                string employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(projectId, employeeId);
            }

            if (!(isManagerOfProject || isAdministrator))
            {
                this.TempData[ErrorMessage] = "You are not authorized to edit interaction to this project.";
                if (isManagerOfProject)
                {
                    return RedirectToAction("Mine", "Employee");
                }

                return RedirectToAction("All", "Project");
            }


            bool isInteractionExists = await this.interactionService.InteractionExistsByProjectIdAsync(projectId);
            if (!isInteractionExists)
            {
                this.TempData[ErrorMessage] = "Interaction does not exist.";
                return RedirectToAction("All", "Interaction");
            }

            AddAndEditInteractionFormModel viewModel =
                await this.interactionService.GetInteractionForEditByProjectIdAsync(projectId, interactionId);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string interactionId, string projectId,
            AddAndEditInteractionFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientAsync(userId);
            bool isAdministrator = this.User.IsAdministrator();

            if (isClient)
            {
                this.TempData[ErrorMessage] = "You are not authorized to edit interaction to this project.";
                return RedirectToAction("Mine", "Client");
            }

            bool isManagerOfProject = false;
            if (isEmployee)
            {
                string employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(projectId, employeeId);
            }

            if (!(isManagerOfProject || isAdministrator))
            {
                this.TempData[ErrorMessage] = "You are not authorized to edit interaction to this project.";
                if (isManagerOfProject)
                {
                    return RedirectToAction("Mine", "Employee");
                }

                return RedirectToAction("All", "Project");
            }

            bool isInteractionExists = await this.interactionService.InteractionExistsByProjectIdAsync(projectId);
            if (!isInteractionExists)
            {
                this.TempData[ErrorMessage] = "Interaction does not exist.";
                return RedirectToAction("All", "Interaction");
            }

            try
            {
                await this.interactionService.EditInteractionByProjectIdAsync(interactionId, formModel);

                this.TempData[SuccessMessage] = $"Interaction edited successfully.";
                return this.RedirectToAction("All", "Interaction", new { id = projectId });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while editing the interaction. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
    }
}