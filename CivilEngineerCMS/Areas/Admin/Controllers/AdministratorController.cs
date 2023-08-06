namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Administrator;

    using static CivilEngineerCMS.Common.GeneralApplicationConstants;
    using static CivilEngineerCMS.Common.NotificationMessagesConstants;

    public class AdministratorController : BaseAdminController
    {
        private readonly IAdministratorService administratorService;


        public AdministratorController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }
        /// <summary>
        /// This method return view for manage administrators
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This method return view for saving administrators
        /// </summary>
        /// <param name="selectedEmployee"></param>
        /// <returns></returns>
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