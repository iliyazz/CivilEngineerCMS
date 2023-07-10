namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Manager;

    using Web.ViewModels.Employee;


    public interface IEmployeeService
    {
        Task<bool> EmployeeExistsByUserIdAsync(string id);
        Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesAsync();
        Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByManagerIdAsync(string id);
        Task<IEnumerable<ProjectSelectManagerFormModel>> AllManagersAsync();
        Task<string> GetManagerIdByUserIdAsync(string userId);
        Task CreateEmployeeAsync(CreateEmployeeFormModel formModel);
    }
}
