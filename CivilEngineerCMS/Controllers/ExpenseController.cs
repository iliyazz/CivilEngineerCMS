namespace CivilEngineerCMS.Web.Controllers
{
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

        public ExpenseController(IExpenseService expensesService, IEmployeeService employeeService,
            IClientService clientService, IProjectService projectService)
        {
            this.expensesService = expensesService;
            this.employeeService = employeeService;
            this.clientService = clientService;
            this.projectService = projectService;
        }
        /// <summary>
        /// This method return details for expense
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string id)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isManagerOfProject = false;
            if (isEmployee)
            {
                var employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
            }

            bool isClient = await this.clientService.IsClientByUserIdAsync(userId);

            bool isClientOfProject = false;
            if (isClient)
            {
                string clientIdByUserId = await this.clientService.GetClientIdByUserIdAsync(userId);
                string clientIdByProjectId = await this.clientService.GetClientIdByProjectIdAsync(id);
                isClientOfProject = string.Equals(clientIdByUserId, clientIdByProjectId,
                    StringComparison.CurrentCultureIgnoreCase);
            }

            if (!(isManagerOfProject || isClientOfProject || this.User.IsAdministrator()))
            {
                this.TempData[ErrorMessage] = "You are not authorized to view expenses to this project.";
                return RedirectToAction("Index", "home");
            }

            if ((isManagerOfProject || this.User.IsAdministrator()) &&
                !await this.expensesService.ExpenseExistsByProjectIdAsync(id))
            {
                this.TempData[ErrorMessage] = "No payments have been made for this project.";
                return RedirectToAction("Add", "Expense", new { id = id });
            }

            if (isClientOfProject && !await this.expensesService.ExpenseExistsByProjectIdAsync(id))
            {
                this.TempData[ErrorMessage] = "No payments have been made for this project.";
                return RedirectToAction("Mine", "Client", new { id = id });
            }

            var viewModel = await this.expensesService.GetExpensesByProjectIdIdAsync(id);
            return View(viewModel);
        }
        /// <summary>
        /// This method return view for add expense
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Add(string id)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isManagerOfProject = false;
            if (isEmployee)
            {
                var employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
            }

            if (!(isManagerOfProject || this.User.IsAdministrator()))
            {
                this.TempData[ErrorMessage] = "You are not authorized to view expenses to this project.";
                return RedirectToAction("Index", "home");
            }

            bool isExpenseExists = await this.expensesService.ExpenseExistsByProjectIdAsync(id);
            if (isExpenseExists)
            {
                this.TempData[ErrorMessage] = "Expense already exists.";
                return RedirectToAction("Details", "Expense", new { id = id });
            }

            AddAndEditExpensesFormModel formModel = new AddAndEditExpensesFormModel
            {
                Date = DateTime.UtcNow,
                ProjectId = Guid.Parse(id)
            };
            return View();
        }
        /// <summary>
        /// This method add expense
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(string id, AddAndEditExpensesFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isManagerOfProject = false;
            if (isEmployee)
            {
                var employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
            }

            if (!(isManagerOfProject || this.User.IsAdministrator()))
            {
                this.TempData[ErrorMessage] = "You are not authorized to add expenses to this project.";
                return RedirectToAction("Index", "home");
            }

            bool isExpenseExists = await this.expensesService.ExpenseExistsByProjectIdAsync(id);
            if (isExpenseExists)
            {
                this.TempData[ErrorMessage] = "Expense already exists.";
                return RedirectToAction("Details", "Expense", new { id = id });
            }

            try
            {
                await this.expensesService.CreateExpenseAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Expense added successfully.";
                return this.RedirectToAction("Details", "Expense", new { id = id });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the expense. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
        /// <summary>
        /// This method return view for edit expense
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isManagerOfProject = false;
            if (isEmployee)
            {
                var employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
            }

            if (!(isManagerOfProject || this.User.IsAdministrator()))
            {
                this.TempData[ErrorMessage] = "You are not authorized to edit expenses to this project.";
                return RedirectToAction("Index", "home");
            }

            bool isExpenseExists = await this.expensesService.ExpenseExistsByProjectIdAsync(id);
            if (!isExpenseExists)
            {
                this.TempData[ErrorMessage] = "Expense does not exist.";
                return RedirectToAction("Add", "Expense");
            }

            AddAndEditExpensesFormModel viewModel = await this.expensesService.GetExpenseForEditByProjectIdAsync(id);
            return this.View(viewModel);
        }
        /// <summary>
        /// This method edit expense
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddAndEditExpensesFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }


            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isManagerOfProject = false;
            if (isEmployee)
            {
                var employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
                isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
            }

            if (!(isManagerOfProject || this.User.IsAdministrator()))
            {
                this.TempData[ErrorMessage] = "You are not authorized to edit expenses to this project.";
                return RedirectToAction("Index", "home");
            }

            bool isExpenseExists = await this.expensesService.ExpenseExistsByProjectIdAsync(id);
            if (!isExpenseExists)
            {
                this.TempData[ErrorMessage] = "Expense does not exist.";
                return RedirectToAction("Add", "Expense");
            }

            try
            {
                await this.expensesService.EditExpenseForEditByProjectIdAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Expense edited successfully.";
                return this.RedirectToAction("Details", "Expense", new { id = id });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while editing the expense. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
    }
}