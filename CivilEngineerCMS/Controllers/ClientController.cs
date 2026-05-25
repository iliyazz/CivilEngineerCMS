namespace CivilEngineerCMS.Web.Controllers
{
    using Data.Models;

    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services.Data.Interfaces;

    using ViewModels.Client;

    using static CivilEngineerCMS.Common.EntityValidationConstants;
    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;

    public class ClientController : BaseController
    {
        private readonly IClientService clientService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ClientController> logger;

        public ClientController(IClientService clientService, UserManager<ApplicationUser> userManager,
            ILogger<ClientController> logger)
        {
            this.clientService = clientService;
            this.userManager = userManager;
            this.logger = logger;
        }
        /// <summary>
        /// Returns all projects associated with the currently logged-in client.
        /// </summary>
        /// <returns>A view with the list of the client's projects, or a redirect to the home page if the user is not a client.</returns>
        public async Task<IActionResult> Mine()
        {
            string userId = this.User.GetId();
            bool clientExists = await this.clientService.IsClientByUserIdAsync(userId);
            if (!clientExists)
            {
                this.TempData[ErrorMessage] = "Client does not exist.";
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<MineClientManagerProjectViewModel> viewModel =
                await this.clientService.AllProjectsByUserIdAsync(this.User.GetId());
            return View(viewModel);
        }

        /// <summary>
        /// Returns the form view for creating a new client.
        /// </summary>
        /// <returns>The create client form view, or a redirect to the home page if the user is not an administrator.</returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }


            CreateClientFormModel formModel = new CreateClientFormModel
            {
                Users = await this.userManager
                    .Users
                    .Select(u => new AllUsersSelectViewModelForClient
                    {
                        Id = u.Id,
                        Email = u.Email
                    })
                    .ToListAsync()
            };

            return this.View();
        }
        /// <summary>
        /// Creates a new client from the submitted form data.
        /// </summary>
        /// <param name="formModel">The form model containing the new client's data.</param>
        /// <returns>Redirects to the client list on success, or returns the form view with validation errors on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateClientFormModel formModel)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            try
            {
                await this.clientService.CreateClientAsync(formModel);

                this.TempData[SuccessMessage] =
                    $"Client {formModel.FirstName} {formModel.LastName} added successfully.";
                return this.RedirectToAction("All", "Client", new{Area = AdminAreaName});
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating client {FirstName} {LastName}.", formModel.FirstName, formModel.LastName);
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the client. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
        /// <summary>
        /// Returns the details view for the client identified by the route id value.
        /// </summary>
        /// <returns>The client details view, or a redirect if the user is not authorized or the client does not exist.</returns>
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                this.ModelState.AddModelError(string.Empty, "You are not authorized to view this page.");
                return this.RedirectToAction("Index", "Home");
            }
            string? clientIdRoutData = (string?)Url.ActionContext.RouteData.Values["id"];

            if (string.IsNullOrWhiteSpace(clientIdRoutData) || !await clientService.ClientExistsByIdAsync(clientIdRoutData))
            {
                this.TempData[ErrorMessage] = "Client does not exist.";
                this.ModelState.AddModelError(string.Empty, "Client does not exist.");
                return this.RedirectToAction("All", "Client",new { Area = AdminAreaName });
            }

            DetailsClientViewModel viewModel = await this.clientService.DetailsClientAsync(clientIdRoutData);
            return this.View(viewModel);
        }
        /// <summary>
        /// Returns the edit form view for the specified client.
        /// </summary>
        /// <param name="id">The unique identifier of the client to edit.</param>
        /// <returns>The edit client form view, or a redirect if the user is not authorized or the client does not exist.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            bool clientExists = await this.clientService.ClientExistsByIdAsync(id.ToString());
            if (!clientExists)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client", new { Area = AdminAreaName });
            }

            try
            {
                EditClientFormModel viewModel = await this.clientService.GetClientForEditByIdAsync(id.ToString());
                this.TempData[WarningMessage] =
                    $"You are going to edit client {viewModel.FirstName} {viewModel.LastName}.";
                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading client {ClientId} for edit.", id);
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client", new { Area = AdminAreaName });
            }
        }
        /// <summary>
        /// Processes the submitted edit form and updates the specified client's data.
        /// </summary>
        /// <param name="id">The unique identifier of the client to update.</param>
        /// <param name="formModel">The form model containing the updated client data.</param>
        /// <returns>Redirects to the client list on success, or returns the form view with errors on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditClientFormModel formModel)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }


            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool clientExists = await this.clientService.ClientExistsByIdAsync(id);
            if (!clientExists)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client", new { Area = AdminAreaName });
            }

            try
            {
                await this.clientService.EditClientByIdAsync(id, formModel);

                this.TempData[SuccessMessage] =
                    $"Client {formModel.FirstName} {formModel.LastName} edited successfully.";
                return this.RedirectToAction("All", "Client", new { Area = AdminAreaName });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error editing client {ClientId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the client. Please try again later or contact administrator!";
                return this.View(formModel);
            }
        }
        /// <summary>
        /// Sets a generic error message and redirects to the home page.
        /// </summary>
        /// <returns>A redirect result to the home page.</returns>
        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Returns the delete confirmation view for the specified client.
        /// </summary>
        /// <param name="id">The unique identifier of the client to delete.</param>
        /// <returns>The delete confirmation view, or a redirect if the user is not authorized or the client does not exist.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            bool clientExists = await this.clientService.ClientExistsByIdAsync(id.ToString());
            if (!clientExists)
            {
                return this.RedirectToAction("All", "Client");
            }

            try
            {
                ClientPreDeleteViewModel viewModel =
                    await this.clientService.GetClientForPreDeleteByIdAsync(id.ToString());
                this.TempData[WarningMessage] =
                    $"You are going to delete client {viewModel.FirstName} {viewModel.LastName}.";
                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading client {ClientId} for delete.", id);
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return GeneralError();
            }
        }

        /// <summary>
        /// Deletes the specified client permanently.
        /// </summary>
        /// <param name="id">The unique identifier of the client to delete.</param>
        /// <param name="formModel">The pre-delete view model containing the client's display data.</param>
        /// <returns>Redirects to the client list on success, or to the home page on unexpected error.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id, ClientPreDeleteViewModel formModel)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }


            bool clientExists = await this.clientService.ClientExistsByIdAsync(id);
            if (!clientExists)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client");
            }

            try
            {
                await this.clientService.DeleteClientByIdAsync(id);

                this.TempData[WarningMessage] =
                    $"Client {formModel.FirstName} {formModel.LastName} deleted successfully.";
                return this.RedirectToAction("All", "Client", new { Area = AdminAreaName });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting client {ClientId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while deleting the client. Please try again later or contact administrator!";
                return GeneralError();
            }
        }
    }
}
