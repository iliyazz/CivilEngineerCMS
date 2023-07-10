namespace CivilEngineerCMS.Web.Controllers
{
    using Data.Models;

    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Services.Data.Interfaces;

    using ViewModels.Client;

    public class ClientController : BaseController
    {
        private readonly IClientService  clientService;
        private readonly UserManager<ApplicationUser> userManager;

        public ClientController(IClientService clientService, UserManager<ApplicationUser> userManager)
        {
            this.clientService = clientService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Mine()
        {
            IEnumerable<MineClientManagerProjectViewModel> viewModel = await this.clientService.AllProjectsByUserIdAsync(this.User.GetId());
            return View(viewModel);
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<AllClientViewModel> viewModel = await this.clientService.AllClientsForViewAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            try
            {
                await this.clientService.CreateClientAsync(formModel);

                this.TempData["SuccessMessage"] = $"Client {formModel.FirstName} {formModel.LastName} added successfully.";
                return this.RedirectToAction("All");
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                                       "An error occurred while adding the client. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }

        public async Task<IActionResult> Details()
        {
            var clientId = (string?)Url.ActionContext.RouteData.Values["id"];
            if (clientId == null)
            {
                this.TempData["ErrorMessage"] = "Client does not exist.";
                this.ModelState.AddModelError(string.Empty, "Client does not exist.");
                return this.RedirectToAction("All");
            }
            //var url = Url.RequestContext.RouteData.Values["id"];
            if (!await clientService.ClientExistsByIdAsync(clientId))
            {
                this.TempData["ErrorMessage"] = "Client does not exist.";
                this.ModelState.AddModelError(string.Empty, "Client does not exist.");
                return this.RedirectToAction("All");
            }
            DetailsClientViewModel viewModel = await this.clientService.DetailsClientAsync(clientId);
            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            bool clientExists = await this.clientService.ClientExistsByIdAsync(id.ToString());
            if (!clientExists)
            {
                this.TempData["ErrorMessage"] = "Client with provided id does not exist.";
                return this.RedirectToAction("All");
            }

            try
            {
                EditClientFormModel viewModel = await this.clientService.GetClientForEditByIdAsync(id.ToString());
                this.TempData["SuccessMessage"] = $"Client {viewModel.FirstName} {viewModel.LastName} edited successfully.";
                return this.View(viewModel);
            }
            catch (Exception _)
            {
                this.TempData["ErrorMessage"] = "Client with provided id does not exist.";
                return this.RedirectToAction("All");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditClientFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool clientExists = await this.clientService.ClientExistsByIdAsync(id);
            if (!clientExists)
            {
                this.TempData["ErrorMessage"] = "Client with provided id does not exist.";
                return this.RedirectToAction("All");
            }

            try
            {
                await this.clientService.EditClientByIdAsync(id, formModel);

                this.TempData["SuccessMessage"] = $"Client {formModel.FirstName} {formModel.LastName} edited successfully.";
                return this.RedirectToAction("All");
            }
            catch (Exception _)
            {
                this.TempData["ErrorMessage"] = "An error occurred while editing the client. Please try again later or contact administrator!";
                return this.View(formModel);
            }
        }


    }
}
/*
        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddAndEditProjectFormModel formModel)
        {


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
 */
