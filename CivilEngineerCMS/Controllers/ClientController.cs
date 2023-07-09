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
            CreateAndEditClientFormModel formModel = new CreateAndEditClientFormModel
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
        public async Task<IActionResult> Create(CreateAndEditClientFormModel formModel)
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
    }
}


