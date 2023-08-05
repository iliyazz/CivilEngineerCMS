using Microsoft.Extensions.Caching.Memory;

namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Employee;

    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    public class EmployeeController : BaseAdminController
    {
        private readonly IEmployeeService employeeService;
        private readonly IMemoryCache memoryCache;

        public EmployeeController(IEmployeeService employeeService, IMemoryCache memoryCache)
        {
            this.employeeService = employeeService;
            this.memoryCache = memoryCache;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            IEnumerable<AllEmployeeViewModel> viewModel = await this.employeeService.AllEmployeesAsync();
            if (viewModel == null)
            {
                viewModel = await this.employeeService.AllEmployeesAsync();
                MemoryCacheEntryOptions cachesOption = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(OnLineUsersCacheExpirationInMinutes));
                this.memoryCache.Set(OnLineEmployeesCacheKey, viewModel, cachesOption);
            }

            //IEnumerable<AllEmployeeViewModel> viewModel = await this.employeeService.AllEmployeesAsync();
            return this.View(viewModel);
        }

    }
}