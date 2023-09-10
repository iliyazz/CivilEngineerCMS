namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Data.Models;
    using CivilEngineerCMS.Services.Data.Models.Project;
    using CivilEngineerCMS.Web.ViewModels.Employee;
    using Microsoft.AspNetCore.Http;

    using Models.Statistics;

    using Web.ViewModels.Project;
    using Task = System.Threading.Tasks.Task;

    public interface IProjectService
    {
        bool StatusExists(string id);
        Task CreateProjectAsync(AddAndEditProjectFormModel formModel/*, string uniqueFileName*/);
        Task<AddAndEditProjectFormModel> GetProjectForEditByIdAsync(string projectId);
        Task<bool> IsManagerOfProjectAsync(string projectId, string managerId);
        Task EditProjectByIdAsync(string projectId, AddAndEditProjectFormModel formModel);
        Task<bool> ProjectExistsByIdAsync(string id);
        Task<ProjectPreDeleteViewModel> GetProjectForPreDeleteByIdAsync(string projectId);
        Task DeleteProjectByIdAsync(string id);
        Task<DetailsProjectViewModel> DetailsByIdProjectAsync(string projectId);
        Task<ProjectAllFilteredAndPagedServiceModel> ProjectAllFilteredAndPagedAsync(ProjectAllQueryModel queryModel);
        Task<StatisticsServiceModel> GetStatisticsAsync();
        Task<IEnumerable<SelectEmployeesForProjectFormModel>> AllEmployeesForProjectAsync(string projectId);
        Task SaveAllEmployeesForProjectAsync(string projectId, IEnumerable<string> idList);
        Task<Project> GetProjectByIdAsync(string projectId);
        Task<string> GetManagerIdByProjectIdAsync(string projectId);
        Task<bool> IsEmployeeOfProjectAsync(string projectId, string employeeId);
        string GetContentType(string fileName);
        string CreateUniqueFileExtension(string fileName);
        Task<byte[]> GetByteArrayFromImage(IFormFile file);
        bool IfFileIsImage(IFormFile file);
    }
}