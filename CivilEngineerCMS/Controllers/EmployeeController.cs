﻿namespace CivilEngineerCMS.Web.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Employee;

    using static Common.NotificationMessagesConstants;

    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllEmployeeViewModel> viewModel = await this.employeeService.AllEmployeesAsync();
            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string? id = this.User.GetId();
            var employeeId = await this.employeeService.GetManagerIdByUserIdAsync(id);
            bool isEmployee = await this.employeeService.EmployeeExistsByIdAsync(employeeId);
            if (!isEmployee)
            {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<MineManagerProjectViewModel> viewModel = await this.employeeService.AllProjectsByManagerIdAsync(employeeId);
            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateEmployeeFormModel formModel = new CreateEmployeeFormModel();

            return this.View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            try
            {
                await this.employeeService.CreateEmployeeAsync(formModel);

                this.TempData[SuccessMessage] =
                    $"Employee {formModel.FirstName} {formModel.LastName} added successfully.";
                return this.RedirectToAction("All");
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while adding the employee. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var employeeId = (string?)Url.ActionContext.RouteData.Values["id"];
            if (employeeId == null)
            {
                this.TempData[ErrorMessage] = "Employee does not exist.";
                this.ModelState.AddModelError(string.Empty, "Employee does not exist.");
                return this.RedirectToAction("All", "Employee");
            }
            if (!await employeeService.EmployeeExistsByIdAsync(employeeId))
            {
                this.TempData[ErrorMessage] = "Employee does not exist.";
                this.ModelState.AddModelError(string.Empty, "Employee does not exist.");
                return this.RedirectToAction("All", "Employee");
            }
            DetailsEmployeeViewModel viewModel = await this.employeeService.DetailsEmployeeAsync(employeeId);
            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            bool employeeExists = await this.employeeService.EmployeeExistsByIdAsync(id.ToString());
            if (!employeeExists)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                EditEmployeeFormModel viewModel = await this.employeeService.GetEmployeeForEditByIdAsync(id.ToString());
                this.TempData[SuccessMessage] = $"Employee {viewModel.FirstName} {viewModel.LastName} edited successfully.";
                return this.View(viewModel);
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditEmployeeFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool employeeExist = await this.employeeService.EmployeeExistsByIdAsync(id);
            if (!employeeExist)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                await this.employeeService.EditEmployeeByIdAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Employee {formModel.FirstName} {formModel.LastName} edited successfully.";
                return this.RedirectToAction("All", "Employee");
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "An error occurred while editing the employee. Please try again later or contact administrator!";
                return this.View(formModel);
            }
        }

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool employeeExists = await this.employeeService.EmployeeExistsByIdAsync(id.ToString());
            if (!employeeExists)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                EmployeePreDeleteViewModel viewModel = await this.employeeService.GetEmployeeForPreDeleteByIdAsync(id.ToString());
                this.TempData[WarningMessage] =
                    $"You are going to delete employee {viewModel.FirstName} {viewModel.LastName}.";
                return this.View(viewModel);
            }
            catch (Exception _)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, EmployeePreDeleteViewModel formModel)
        {

            bool employeeExists = await this.employeeService.EmployeeExistsByIdAsync(id);
            if (!employeeExists)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                await this.employeeService.DeleteEmployeeByIdAsync(id);

                this.TempData[WarningMessage] =
                    $"Employee {formModel.FirstName} {formModel.LastName} deleted successfully.";
                return this.RedirectToAction("All", "Employee");
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] =
                    "An error occurred while deleting the employee. Please try again later or contact administrator!";
                return GeneralError();
            }
        }
    }
}

