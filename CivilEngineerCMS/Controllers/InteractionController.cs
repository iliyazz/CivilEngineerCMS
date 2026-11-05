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
        private readonly ILogger<InteractionController> logger;

        public InteractionController(IInteractionService interactionService, IEmployeeService employeeService,
            IClientService clientService, IProjectService projectService, ILogger<InteractionController> logger)
        {
            this.interactionService = interactionService;
            this.employeeService = employeeService;
            this.clientService = clientService;
            this.projectService = projectService;
            this.logger = logger;
        }
        /// <summary>
        /// Returns a view listing all interactions for the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project whose interactions are displayed.</param>
        /// <returns>A view with all project interactions, or a redirect if the user is not authorized or no interactions exist yet.</returns>
        [HttpGet]
        public async Task<IActionResult> All(string id)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientByUserIdAsync(userId);
            bool isAdministrator = this.User.IsAdministrator();

            bool isClientOfProject = false;
            if (isClient)
            {
                string clientIdByUserId = await this.clientService.GetClientIdByUserIdAsync(userId);
                string? clientIdByProjectId = await this.clientService.GetClientIdByProjectIdAsync(id);
                isClientOfProject = clientIdByProjectId != null &&
                    string.Equals(clientIdByUserId, clientIdByProjectId, StringComparison.OrdinalIgnoreCase);
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
        /// <summary>
        /// Returns the form view for adding a new interaction to the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project the interaction belongs to.</param>
        /// <returns>The add interaction form view, or a redirect if the user is not authorized to add interactions.</returns>
        [HttpGet]
        public async Task<IActionResult> Add(string id)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientByUserIdAsync(userId);
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
        /// <summary>
        /// Creates a new interaction record for the specified project from the submitted form data.
        /// </summary>
        /// <param name="id">The unique identifier of the project the interaction belongs to.</param>
        /// <param name="formModel">The form model containing the interaction data.</param>
        /// <returns>Redirects to the interaction list on success, or returns the form view with errors on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(string id, AddAndEditInteractionFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientByUserIdAsync(userId);
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
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating interaction for project {ProjectId}.", id);
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the interaction. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
        /// <summary>
        /// Returns the edit form view for the specified interaction within a project.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project the interaction belongs to.</param>
        /// <param name="interactionId">The unique identifier of the interaction to edit.</param>
        /// <returns>The edit interaction form view, or a redirect if the user is not authorized or the interaction does not exist.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string projectId, string interactionId)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isClient = await this.clientService.IsClientByUserIdAsync(userId);
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
                return RedirectToAction("Mine", "Employee");
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
        /// <summary>
        /// Processes the submitted edit form and updates the specified interaction record.
        /// </summary>
        /// <param name="interactionId">The unique identifier of the interaction to update.</param>
        /// <param name="projectId">The unique identifier of the project the interaction belongs to.</param>
        /// <param name="formModel">The form model containing the updated interaction data.</param>
        /// <returns>Redirects to the interaction list on success, or returns the form view with errors on failure.</returns>
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
            bool isClient = await this.clientService.IsClientByUserIdAsync(userId);
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
                return RedirectToAction("Mine", "Employee");
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
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error editing interaction {InteractionId} for project {ProjectId}.", interactionId, projectId);
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while editing the interaction. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
    }
}