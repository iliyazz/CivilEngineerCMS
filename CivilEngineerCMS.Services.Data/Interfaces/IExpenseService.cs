namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Employee;

    using Web.ViewModels.Expenses;

    public interface IExpenseService
    {
        Task<AddAndEditExpensesFormModel> GetExpensesByProjectIdIdAsync(string projectId);
        Task<bool> ExpenseExistsByProjectIdAsync(string projectId);
        Task CreateExpenseAsync(string id, AddAndEditExpensesFormModel formModel);
        Task<AddAndEditExpensesFormModel> GetExpenseForEditByProjectIdAsync(string projectId);
        Task EditExpenseForEditByProjectIdAsync(string projectId, AddAndEditExpensesFormModel formModel);
    }
}
