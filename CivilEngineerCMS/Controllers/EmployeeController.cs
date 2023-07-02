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

        //public async Task<IActionResult> Mine()
        //{
        //    string? userId = this.User.GetId();
        //    bool isEmployee = await this.employeeService.EmployeeExistsByUserIdAsync(userId);
        //    if (!isEmployee)
        //    {
        //        return this.BadRequest();
        //    }

        //    return this.View();
        //}

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllEmployeeViewModel> viewModel = await this.employeeService.AllEmployeesAsync();
            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string? userId = this.User.GetId();
            bool isEmployee = await this.employeeService.EmployeeExistsByUserIdAsync(userId);
            if (!isEmployee)
            {
                return this.BadRequest();
            }

            IEnumerable<MineManagerProjectViewModel> viewModel = await this.employeeService.AllProjectsByManagerIdAsync(userId);
            return this.View(viewModel);
        }
    }
}