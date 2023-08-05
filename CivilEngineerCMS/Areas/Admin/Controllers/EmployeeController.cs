namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Employee;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;

    public class EmployeeController : BaseAdminController
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
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
            return this.View(viewModel);
        }

    }
}