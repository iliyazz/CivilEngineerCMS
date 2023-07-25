namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Client;
    using Web.ViewModels.Expenses;
    using Task = System.Threading.Tasks.Task;

    public class ExpenseService : IExpenseService
    {
        private readonly CivilEngineerCmsDbContext dbContext;
        //private readonly IProjectService projectService;

        public ExpenseService(CivilEngineerCmsDbContext dbContext/*, IProjectService projectService*/)
        {
            this.dbContext = dbContext;
            //this.projectService = projectService;
        }


        public async Task<AddAndEditExpensesFormModel> GetExpensesByProjectIdIdAsync(string projectId)
        {


            AddAndEditExpensesFormModel expenses = await this.dbContext
                .Expenses
                .Where(e => e.ProjectId.ToString() == projectId)
                .Select(e => new AddAndEditExpensesFormModel
                {
                    ProjectId = e.ProjectId,
                    Amount = e.Amount,
                    TotalAmount = e.TotalAmount,
                    Date = e.Date,
                })
                .FirstAsync();

            var result = new AddAndEditExpensesFormModel
            {
                ProjectId = expenses.ProjectId,
                Amount = expenses.Amount,
                TotalAmount = expenses.TotalAmount,
                Date = expenses.Date,
            };
            return result;
        }

        public Task<bool> ExpenseExistsByProjectIdAsync(string projectId)
        {
            return this.dbContext.Expenses.AnyAsync(e => e.ProjectId.ToString() == projectId);
        }

        public async Task CreateExpenseAsync(string id, AddAndEditExpensesFormModel formModel)
        {
            Expense expense = new Expense
            {
                ProjectId = Guid.Parse(id),
                Amount = formModel.Amount,
                TotalAmount = formModel.TotalAmount,
                Date = DateTime.UtcNow,

            };
            await this.dbContext.Expenses.AddAsync(expense);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<AddAndEditExpensesFormModel> GetExpenseForEditByProjectIdAsync(string projectId)
        {
            Expense expense = await this.dbContext.Expenses
                .Where(e => e.ProjectId.ToString() == projectId)
                .FirstAsync();
            AddAndEditExpensesFormModel result = new AddAndEditExpensesFormModel
            {
                Amount = expense.Amount,
                TotalAmount = expense.TotalAmount,
                Date = expense.Date
            };
            return result;
        }

        public async Task EditExpenseForEditByProjectIdAsync(string projectId, AddAndEditExpensesFormModel formModel)
        {
            Expense expense = await this.dbContext
                .Expenses
                .Where(e => e.ProjectId.ToString() == projectId)
                .FirstAsync();
            expense.Amount = formModel.Amount;
            expense.TotalAmount = formModel.TotalAmount;
            expense.Date = DateTime.UtcNow;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
