
namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Employee;
    using Web.ViewModels.Administrator;

    public interface IAdministratorService
    {
        //Task AssignAdministratorRoleAsync(string userId);
        //Task RemoveAdministratorRoleAsync(string userId);
        Task<IEnumerable<SelectEmployeesForAdministratorFormModel>> AllEmployeesForAdministratorAsync();
        Task SaveAllEmployeesForAdministratorAsync(string currentUserId, IEnumerable<string> idList);
    }
}