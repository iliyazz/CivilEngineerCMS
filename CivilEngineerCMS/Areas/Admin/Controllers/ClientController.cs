using System.Collections;
using Microsoft.Extensions.Caching.Memory;

namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Client;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;

    public class ClientController : BaseAdminController
    {
        private readonly IClientService clientService;
        private readonly IMemoryCache memoryCache;

        public ClientController(IClientService clientService, IMemoryCache memoryCache)
        {
            this.clientService = clientService;
            this.memoryCache = memoryCache;
        }
        /// <summary>
        /// This method return view for all clients
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> All()
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<AllClientViewModel> viewModel = this.memoryCache.Get<IEnumerable<AllClientViewModel>>(OnLineClientsCacheKey);
            if (viewModel == null)
            {
                viewModel = await this.clientService.AllClientsForViewAsync();
                MemoryCacheEntryOptions cachesOption = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(OnLineUsersCacheExpirationInMinutes));

                this.memoryCache.Set(OnLineClientsCacheKey, viewModel, cachesOption);
            }

            //IEnumerable<AllClientViewModel> viewModel = await this.clientService.AllClientsForViewAsync();
            return View(viewModel);
        }

    }
}