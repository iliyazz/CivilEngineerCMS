namespace CivilEngineerCMS.Web.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Employee;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;

    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            this.employeeService = employeeService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns all projects where the current user acts as a manager.
        /// </summary>
        /// <returns>A view with the list of managed projects, or a redirect to the home page if the user is not a manager.</returns>
        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string id = this.User.GetId();
            var employeeId = await this.employeeService.GetManagerIdByUserIdAsync(id);
            if (employeeId == null || !await this.employeeService.EmployeeExistsByIdAsync(employeeId))
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<MineManagerProjectViewModel> viewModel =
                await this.employeeService.AllProjectsByManagerIdAsync(employeeId);
            return this.View(viewModel);
        }
        /// <summary>
        /// Returns all projects to which the current user is assigned as an employee (worker).
        /// </summary>
        /// <returns>A view with the list of assigned projects, or a redirect to the home page if the user is not an employee.</returns>
        [HttpGet]
        public async Task<IActionResult> MineByEmployeeId()
        {
            string id = this.User.GetId();
            var employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(id);
            bool isEmployee = await this.employeeService.EmployeeExistsByIdAsync(employeeId);
            if (!isEmployee)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<MineManagerProjectViewModel> viewModel =
                await this.employeeService.AllProjectsByEmployeeIdAsync(employeeId);
            return this.View(viewModel);
        }
        /// <summary>
        /// Returns the form view for creating a new employee.
        /// </summary>
        /// <returns>The create employee form view, or a redirect to the home page if the user is not an administrator.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            CreateEmployeeFormModel formModel = new CreateEmployeeFormModel();

            return this.View(formModel);
        }
        /// <summary>
        /// Creates a new employee from the submitted form data.
        /// </summary>
        /// <param name="formModel">The form model containing the new employee's data.</param>
        /// <returns>Redirects to the employee list on success, or returns the form view with validation errors on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeFormModel formModel)
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
                await this.employeeService.CreateEmployeeAsync(formModel);

                this.TempData[SuccessMessage] =
                    $"Employee {formModel.FirstName} {formModel.LastName} added successfully.";
                return this.RedirectToAction("All", "Employee", new{Area = AdminAreaName});
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating employee {FirstName} {LastName}.", formModel.FirstName, formModel.LastName);
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the employee. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
        /// <summary>
        /// Returns the details view for the employee identified by the route id value.
        /// </summary>
        /// <returns>The employee details view, or a redirect to the employee list if the employee does not exist.</returns>
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
        /// <summary>
        /// Returns the edit form view for the specified employee.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to edit.</param>
        /// <returns>The edit employee form view, or a redirect if the user is not authorized or the employee does not exist.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            bool employeeExists = await this.employeeService.EmployeeExistsByIdAsync(id.ToString());
            if (!employeeExists)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                EditEmployeeFormModel viewModel = await this.employeeService.GetEmployeeForEditByIdAsync(id.ToString());
                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading employee {EmployeeId} for edit.", id);
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }
        }
        /// <summary>
        /// Processes the submitted edit form and updates the specified employee's data.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to update.</param>
        /// <param name="formModel">The form model containing the updated employee data.</param>
        /// <returns>Redirects to the employee list on success, or returns the form view with errors on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditEmployeeFormModel formModel)
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

            bool employeeExist = await this.employeeService.EmployeeExistsByIdAsync(id);
            if (!employeeExist)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                await this.employeeService.EditEmployeeByIdAsync(id, formModel);

                this.TempData[SuccessMessage] =
                    $"Employee {formModel.FirstName} {formModel.LastName} edited successfully.";
                return this.RedirectToAction("All", "Employee", new { Area = AdminAreaName });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error editing employee {EmployeeId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the employee. Please try again later or contact administrator!";
                return this.View(formModel);
            }
        }
        /// <summary>
        /// Sets a generic error message and redirects to the home page.
        /// </summary>
        /// <returns>A redirect result to the home page.</returns>
        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Returns the delete confirmation view for the specified employee.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to delete.</param>
        /// <returns>The delete confirmation view, or a redirect if the user is not authorized or the employee does not exist.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            bool employeeExists = await this.employeeService.EmployeeExistsByIdAsync(id.ToString());
            if (!employeeExists)
            {
                this.TempData[ErrorMessage] = "Employee with provided id does not exist.";
                return this.RedirectToAction("All", "Employee");
            }

            try
            {
                EmployeePreDeleteViewModel viewModel =
                    await this.employeeService.GetEmployeeForPreDeleteByIdAsync(id.ToString());
                this.TempData[WarningMessage] =
                    $"You are going to delete employee {viewModel.FirstName} {viewModel.LastName}.";
                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading employee {EmployeeId} for delete.", id);
                return GeneralError();
            }
        }
        /// <summary>
        /// Deletes the specified employee permanently.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to delete.</param>
        /// <param name="formModel">The pre-delete view model containing the employee's display data.</param>
        /// <returns>Redirects to the employee list on success, or to the home page on unexpected error.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id, EmployeePreDeleteViewModel formModel)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

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
                return this.RedirectToAction("All", "Employee", new { Area = AdminAreaName });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting employee {EmployeeId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while deleting the employee. Please try again later or contact administrator!";
                return GeneralError();
            }
        }
    }
}
