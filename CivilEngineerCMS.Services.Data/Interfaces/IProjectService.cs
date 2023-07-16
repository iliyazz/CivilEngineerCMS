namespace CivilEngineerCMS.Services.Data.Interfaces;

using CivilEngineerCMS.Services.Data.Models.Project;
using CivilEngineerCMS.Web.ViewModels.Client;
using CivilEngineerCMS.Web.ViewModels.Employee;

using Web.ViewModels.Project;

public interface IProjectService
{
    Task<IEnumerable<AllProjectViewModel>> AllProjectsAsync();
    Task<bool> ManagerExistsByUserIdAsync(string id);
    Task<bool> ClientExistsByUserIdAsync(string id);
    Task<bool> EmployeeExistsByUserIdAsync(string id);
    bool StatusExists(string id);
    Task CreateProjectAsync(AddAndEditProjectFormModel formModel);
    Task<AddAndEditProjectFormModel> GetProjectForEditByIdAsync(string projectId);
    Task<bool> IsManagerOfProjectAsync(string userId, string managerId);
    Task EditProjectByIdAsync(string projectId, AddAndEditProjectFormModel formModel);
    Task<IEnumerable<MineViewModel>> AllProjectsByManagerIdAsync(string userId);
    Task<bool> ProjectExistsByIdAsync(string id);
    Task<ProjectPreDeleteViewModel> GetProjectForPreDeleteByIdAsync(string projectId);
    Task DeleteProjectByIdAsync(string id);
    Task<DetailsProjectViewModel> DetailsByIdProjectAsync(string projectId);
    Task<ProjectAllFilteredAndPagedServiceModel> ProjectAllFilteredAndPagedAsync(ProjectAllQueryModel queryModel);

}