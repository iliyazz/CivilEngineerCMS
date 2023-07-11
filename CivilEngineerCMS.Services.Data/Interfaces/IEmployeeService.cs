namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Manager;

    using Web.ViewModels.Employee;


    public interface IEmployeeService
    {
        Task<bool> EmployeeExistsByIdAsync(string id);
        Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesAsync();
        Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByManagerIdAsync(string id);
        Task<IEnumerable<ProjectSelectManagerFormModel>> AllManagersAsync();
        Task<string> GetManagerIdByUserIdAsync(string userId);
        Task CreateEmployeeAsync(CreateEmployeeFormModel formModel);
        Task<DetailsEmployeeViewModel> DetailsEmployeeAsync(string employeeId);
        Task<EditEmployeeFormModel> GetEmployeeForEditByIdAsync(string employeeId);
        Task EditEmployeeByIdAsync(string employeeId, EditEmployeeFormModel formModel);

        Task<EmployeePreDeleteViewModel> GetEmployeeForPreDeleteByIdAsync(string employeeId);
        Task DeleteEmployeeByIdAsync(string employeeId);
    }
}
