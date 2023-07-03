namespace CivilEngineerCMS.Services.Data.Interfaces;

using Web.ViewModels.Project;

public interface IProjectService
{
    Task<IEnumerable<AllProjectViewModel>> AllProjectsAsync();
    Task<bool> ManagerExistsByUserIdAsync(string id);
    Task<bool> ClientExistsByUserIdAsync(string id);
    Task<bool> EmployeeExistsByUserIdAsync(string id);
    bool StatusExists(string id);
    Task CreateProjectAsync(AddProjectFormModel formModel);

}