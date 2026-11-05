namespace CivilEngineerCMS.Web.Controllers
{
    using CivilEngineerCMS.Web.ViewModels.Interaction;

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
        private readonly ILogger<ExpenseController> logger;

        public ExpenseController(IExpenseService expensesService, IEmployeeService employeeService,
            IClientService clientService, IProjectService projectService, ILogger<ExpenseController> logger)
        {
            this.expensesService = expensesService;
            this.employeeService = employeeService;
            this.clientService = clientService;
            this.projectService = projectService;
            this.logger = logger;
        }
        /// <summary>
        /// Returns the expense details view for the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project whose expenses are displayed.</param>
        /// <returns>The expense details view, or a redirect if the user is not authorized or no expenses exist for the project.</returns>
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
                string? clientIdByProjectId = await this.clientService.GetClientIdByProjectIdAsync(id);
                isClientOfProject = clientIdByProjectId != null &&
                    string.Equals(clientIdByUserId, clientIdByProjectId, StringComparison.OrdinalIgnoreCase);
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
        /// Returns the form view for adding an expense record to the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project the expense belongs to.</param>
        /// <returns>The add expense form view, or a redirect if the user is not authorized or an expense already exists for the project.</returns>
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
        /// Creates a new expense record for the specified project from the submitted form data.
        /// </summary>
        /// <param name="id">The unique identifier of the project the expense belongs to.</param>
        /// <param name="formModel">The form model containing expense data.</param>
        /// <returns>Redirects to the expense details view on success, or returns the form view with errors on failure.</returns>
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
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating expense for project {ProjectId}.", id);
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the expense. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
        /// <summary>
        /// Returns the edit form view for the expense record associated with the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project whose expense is being edited.</param>
        /// <returns>The edit expense form view, or a redirect if the user is not authorized or the expense does not exist.</returns>
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
        /// Processes the submitted edit form and updates the expense record for the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project whose expense is being updated.</param>
        /// <param name="formModel">The form model containing the updated expense data.</param>
        /// <returns>Redirects to the expense details view on success, or returns the form view with errors on failure.</returns>
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
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error editing expense for project {ProjectId}.", id);
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while editing the expense. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> All(string id)
        //{
        //    string userId = this.User.GetId();
        //    bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
        //    bool isClient = await this.clientService.IsClientByUserIdAsync(userId);
        //    bool isAdministrator = this.User.IsAdministrator();
        //    bool isClientOfProject = false;
        //    if (isClient)
        //    {
        //        string clientIdByUserId = await this.clientService.GetClientIdByUserIdAsync(userId);
        //        string clientIdByProjectId = await this.clientService.GetClientIdByProjectIdAsync(id);
        //        isClientOfProject = string.Equals(clientIdByUserId, clientIdByProjectId,
        //            StringComparison.CurrentCultureIgnoreCase);
        //    }

        //    bool isManagerOfProject = false;
        //    bool isEmployeeOfProject = false;
        //    if (isEmployee)
        //    {
        //        string employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(userId);
        //        isManagerOfProject = await this.projectService.IsManagerOfProjectAsync(id, employeeId);
        //        isEmployeeOfProject = await this.projectService.IsEmployeeOfProjectAsync(id, employeeId);
        //    }

        //    if (!(isEmployeeOfProject || isManagerOfProject || isClientOfProject || isAdministrator))
        //    {
        //        this.TempData[ErrorMessage] = "You are not authorized to view expenses to this project.";
        //        return RedirectToAction("Index", "home");
        //    }

        //    if ((isEmployeeOfProject || isManagerOfProject || isAdministrator)
        //        && !await this.expensesService.ExpenseExistsByProjectIdAsync(id))
        //    {
        //        this.TempData[InfoMessage] = "There are no expenses about this project. Create the first one.";
        //        return RedirectToAction("Add", "Expenses", new { id = id });
        //    }

        //    if (isClientOfProject && !await this.expensesService.ExpenseExistsByProjectIdAsync(id))
        //    {
        //        this.TempData[InfoMessage] = "There are no expenses about this project.";
        //        return RedirectToAction("Mine", "Client", new { id = id });
        //    }

        //    IEnumerable<AddAndEditExpensesFormModel> viewModel = await this.expensesService.AllExpensesByProjectAdAsync(id);
        //    return View(viewModel);
        //}
    }
}
