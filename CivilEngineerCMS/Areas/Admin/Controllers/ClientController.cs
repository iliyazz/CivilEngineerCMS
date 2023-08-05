namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
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

    public class ClientController : BaseAdminController
    {
        private readonly IClientService clientService;
        private readonly UserManager<ApplicationUser> userManager;

        public ClientController(IClientService clientService, UserManager<ApplicationUser> userManager)
        {
            this.clientService = clientService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All()
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<AllClientViewModel> viewModel = await this.clientService.AllClientsForViewAsync();
            return View(viewModel);
        }

    }
}