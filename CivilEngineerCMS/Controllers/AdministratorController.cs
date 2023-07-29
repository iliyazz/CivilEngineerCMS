namespace CivilEngineerCMS.Web.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Administrator;

    using static CivilEngineerCMS.Common.GeneralApplicationConstants;
    using static CivilEngineerCMS.Common.NotificationMessagesConstants;

    public class AdministratorController : BaseController
    {
        private readonly IAdministratorService administratorService;
        private readonly IEmployeeService employeesService;
        private readonly IProjectService projectService;

        public AdministratorController(IEmployeeService employeeService, IProjectService projectService,
            IAdministratorService administratorService)
        {
            this.employeesService = employeeService;
            this.projectService = projectService;
            this.administratorService = administratorService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageAdministrators(string userId)
        {
            if (!this.User.IsInRole(AdministratorRoleName))
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                IEnumerable<SelectEmployeesForAdministratorFormModel> employees =
                    await this.administratorService.AllEmployeesForAdministratorAsync();
                return this.View(employees);
            }
            catch (Exception e)
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageAdministrators(IEnumerable<string> selectedEmployee)
        {
            if (!this.User.IsInRole(AdministratorRoleName))
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                string currentUserId = this.User.GetId();
                await this.administratorService.SaveAllEmployeesForAdministratorAsync(currentUserId, selectedEmployee);
                this.TempData[SuccessMessage] = "Successfully updated administrators!";
                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return this.GeneralError();
            }
        }

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}