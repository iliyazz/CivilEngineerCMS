
namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using Web.ViewModels.Administrator;

    public interface IAdministratorService
    {
        Task<IEnumerable<SelectEmployeesForAdministratorFormModel>> AllEmployeesForAdministratorAsync();
        Task SaveAllEmployeesForAdministratorAsync(string currentUserId, IEnumerable<string> idList);
    }
}