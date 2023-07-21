namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Manager;

    using Web.ViewModels.Employee;


    public interface IEmployeeService
    {
        Task<bool> EmployeeExistsByIdAsync(string id);
        Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesAsync();
        Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByManagerIdAsync(string id);
        Task<IEnumerable<SelectEmployeesAndManagerForProjectFormModel>> AllEmployeesAndManagersAsync();
        Task<bool> EmployeeExistsByUserIdAsync(string id);
        Task<string> GetManagerIdByUserIdAsync(string userId);
        Task CreateEmployeeAsync(CreateEmployeeFormModel formModel);
        Task<DetailsEmployeeViewModel> DetailsEmployeeAsync(string employeeId);
        Task<EditEmployeeFormModel> GetEmployeeForEditByIdAsync(string employeeId);
        Task EditEmployeeByIdAsync(string employeeId, EditEmployeeFormModel formModel);
        Task<EmployeePreDeleteViewModel> GetEmployeeForPreDeleteByIdAsync(string employeeId);
        Task DeleteEmployeeByIdAsync(string employeeId);
        Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesByProjectIdAsync(string projectId);
        Task<bool> IsEmployeeInProjectAsync(string projectId, string employeeId);
        Task<bool> IsEmployeeAsync(string userId);
        Task<string> GetEmployeeIdByUserIdAsync(string userId);
        Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByEmployeeIdAsync(string id);

    }
}
