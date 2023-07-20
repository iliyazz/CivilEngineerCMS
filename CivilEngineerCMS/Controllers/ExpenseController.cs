namespace CivilEngineerCMS.Web.Controllers
{
    using Data.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using ViewModels.Expenses;
    using static Common.NotificationMessagesConstants;

    public class ExpenseController : BaseController
    {
        private readonly IExpenseService expensesService;
        private readonly IEmployeeService employeeService;
        private readonly IClientService clientService;
        private readonly IProjectService projectService;

        public ExpenseController(IExpenseService expensesService, IEmployeeService employeeService, IClientService clientService, IProjectService projectService)
        {
            this.expensesService = expensesService;
            this.employeeService = employeeService;
            this.clientService = clientService;
            this.projectService = projectService;
        }


        public async Task<IActionResult> Details(string id)
        {
            if (!await this.expensesService.ExpenseExistsByProjectIdAsync(id))
            {
                //this.TempData[ErrorMessage] = "No payments have been made for this project.";
                return RedirectToAction("Add", "Expense", new { id = id });
            }
            Project project = await this.projectService.GetProjectByIdAsync(id);

            string userId = this.User.GetId();
            string managerId = await this.employeeService.GetManagerIdByUserIdAsync(userId);
            bool isManagerOfProject = project.ManagerId.ToString() == managerId;
            string clientId = await this.clientService.GetClientIdByProjectIdAsync(id);
            bool isClientOfProject = project.ClientId.ToString() == clientId;

            if (!(isManagerOfProject || isClientOfProject))
            {
                return RedirectToAction("Index", "home");
            }


            var viewModel = await this.expensesService.GetExpensesByProjectIdIdAsync(id);


            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add(string id)
        {
            bool isExpenseExists = await this.expensesService.ExpenseExistsByProjectIdAsync(id);
            if (isExpenseExists)
            {
                this.TempData[ErrorMessage] = "Expense already exists.";
                return RedirectToAction("Details", "Expense", new { id = id });
            }

            AddAndEditExpensesFormModel formModel =  new AddAndEditExpensesFormModel
            {
                Date = DateTime.UtcNow,
                ProjectId = Guid.Parse(id)
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string id, AddAndEditExpensesFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            try
            {
                await this.expensesService.CreateExpenseAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Expense added successfully.";
                return this.RedirectToAction("Details", "Expense", new { id = id });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while adding the expense. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool isExpenseExists = await this.expensesService.ExpenseExistsByProjectIdAsync(id);
            if (!isExpenseExists)
            {
                this.TempData[ErrorMessage] = "Expense does not exist.";
                return RedirectToAction("Details", "Expense");
            }

            AddAndEditExpensesFormModel viewModel = await this.expensesService.GetExpenseForEditByProjectIdAsync(id);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddAndEditExpensesFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            try
            {
                await this.expensesService.EditExpenseForEditByProjectIdAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Expense edited successfully.";
                return this.RedirectToAction("Details", "Expense", new { id = id });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty, "An error occurred while editing the expense. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
    }
}
/*

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            bool clientExists = await this.clientService.ClientExistsByIdAsync(id.ToString());
            if (!clientExists)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client");
            }

            try
            {
                EditClientFormModel viewModel = await this.clientService.GetClientForEditByIdAsync(id.ToString());
                this.TempData[WarningMessage] = $"You are going to edit client {viewModel.FirstName} {viewModel.LastName}.";
                return this.View(viewModel);
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client");
            }

        }
*/
/*

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditClientFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool clientExists = await this.clientService.ClientExistsByIdAsync(id);
            if (!clientExists)
            {
                this.TempData[ErrorMessage] = "Client with provided id does not exist.";
                return this.RedirectToAction("All", "Client");
            }

            try
            {
                await this.clientService.EditClientByIdAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Client {formModel.FirstName} {formModel.LastName} edited successfully.";
                return this.RedirectToAction("All", "Client");
            }
            catch (Exception _)
            {
                this.TempData[ErrorMessage] = "An error occurred while editing the client. Please try again later or contact administrator!";
                return this.View(formModel);
            }
        }

 */