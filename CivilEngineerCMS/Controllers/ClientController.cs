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

        public ClientController(IClientService clientService, UserManager<ApplicationUser> userManager)
        {
            this.clientService = clientService;
            this.userManager = userManager;
        }
        /// <summary>
        /// This method return all projects for current client
        /// </summary>
        /// <returns></returns>
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
        /// This method return view for creating new client
        /// </summary>
        /// <returns></returns>
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
        /// This method create new client
        /// </summary>
        /// <param name="formModel"></param>
        /// <returns></returns>
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
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the client. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
        /// <summary>
        /// This method return details for client
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var clientId = (string?)Url.ActionContext.RouteData.Values["id"];
            if (clientId == null)
            {
                this.TempData[ErrorMessage] = "Client does not exist.";
                this.ModelState.AddModelError(string.Empty, "Client does not exist.");
                return this.RedirectToAction("All", "Client");
            }

            if (!await clientService.ClientExistsByIdAsync(clientId))
            {
                this.TempData[ErrorMessage] = "Client does not exist.";
                this.ModelState.AddModelError(string.Empty, "Client does not exist.");
                return this.RedirectToAction("All", "Client");
            }

            DetailsClientViewModel viewModel = await this.clientService.DetailsClientAsync(clientId);
            return this.View(viewModel);
        }
        /// <summary>
        /// This method return view for editing client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                return this.RedirectToAction("All", "Client");
            }

            try
            {
                EditClientFormModel viewModel = await this.clientService.GetClientForEditByIdAsync(id.ToString());
                this.TempData[WarningMessage] =
                    $"You are going to edit client {viewModel.FirstName} {viewModel.LastName}.";
                return this.View(viewModel);
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client");
            }
        }
        /// <summary>
        /// This method edit client
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
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
                return this.RedirectToAction("All", "Client");
            }

            try
            {
                await this.clientService.EditClientByIdAsync(id, formModel);

                this.TempData[SuccessMessage] =
                    $"Client {formModel.FirstName} {formModel.LastName} edited successfully.";
                return this.RedirectToAction("All", "Client", new { Area = AdminAreaName });
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the client. Please try again later or contact administrator!";
                return this.View(formModel);
            }
        }
        /// <summary>
        /// This method return general error
        /// </summary>
        /// <returns></returns>
        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// This method return view for deleting client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return GeneralError();
            }
        }/// <summary>
        /// This method delete client
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>

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
            catch (Exception _)
            {
                this.TempData[ErrorMessage] =
                    "An error occurred while deleting the client. Please try again later or contact administrator!";
                return GeneralError();
            }
        }
    }
}