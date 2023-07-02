namespace CivilEngineerCMS.Services.Data.Interfaces;

using Web.ViewModels.Project;

public interface IProjectService
{
    Task<IEnumerable<AllProjectViewModel>> AllProjectsAsync();
    Task<bool> ManagerExistsByUserIdAsync(string id);
}