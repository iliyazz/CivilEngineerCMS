namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using Web.ViewModels.Employee;


    public interface IEmployeeService
    {
        Task<bool> EmployeeExistsByUserIdAsync(string id);
        Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesAsync();
        Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByManagerIdAsync(string userId);
        

    }
}
