namespace CivilEngineerCMS.Web.Controllers
{
    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Employee;

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
            bool isEmployee = await this.employeeService.EmployeeExistsByUserIdAsync(id);
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

                this.TempData["SuccessMessage"] =
                    $"Employee {formModel.FirstName} {formModel.LastName} added successfully.";
                return this.RedirectToAction("All");
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while adding the employee. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
    }
}

