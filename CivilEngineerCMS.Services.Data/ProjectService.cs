namespace CivilEngineerCMS.Services.Data;

using System.Globalization;

using CivilEngineerCMS.Data;
using CivilEngineerCMS.Data.Models;

using Common;

using Interfaces;

using Microsoft.EntityFrameworkCore;

using Web.ViewModels.Project;

using Task = System.Threading.Tasks.Task;

public class ProjectService : IProjectService
{
    private readonly CivilEngineerCmsDbContext dbContext;

    public ProjectService(CivilEngineerCmsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<AllProjectViewModel>> AllProjectsAsync()
    {
        IEnumerable<AllProjectViewModel> allProjects = await this.dbContext
            .Projects
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.ProjectCreatedDate)
            .Include(x => x.Client)
            .Include(x => x.Manager)
            .Select(p => new AllProjectViewModel
            {
                ProjectCreatedDate = p.ProjectCreatedDate,
                Name = p.Name,
                ClientName = $"{p.Client.FirstName} {p.Client.LastName}",
                ManagerName = $"{p.Manager.FirstName} {p.Manager.LastName}"
            })
            .ToListAsync();
        return allProjects;
    }

    public async Task<bool> ManagerExistsByUserIdAsync(string id)
    {
        bool managerExists = await this.dbContext
            .Projects
            .Where(x => x.IsActive)
            .AnyAsync(x => x.Id.ToString() == id);
        return managerExists;
    }

    public async Task<bool> ClientExistsByUserIdAsync(string id)
    {
        bool clientExists = await this.dbContext
            .Clients
            .AnyAsync(x => x.Id.ToString() == id);
        return clientExists;
    }

    public async Task<bool> EmployeeExistsByUserIdAsync(string id)
    {
        bool employeeExists = await this.dbContext
            .Employees
            .AnyAsync(x => x.Id.ToString() == id);
        return employeeExists;
    }

    public bool StatusExists(string id)
    {
        bool statusExist = Enum.IsDefined(typeof(ProjectStatusEnums), id);
        return statusExist;
    }

    public async Task CreateProjectAsync(AddAndEditProjectFormModel formModel)
    {
        Project project = new Project
        {
            Name = formModel.Name,
            Description = formModel.Description,
            ClientId = formModel.ClientId,
            ManagerId = formModel.ManagerId,
            UrlPicturePath = formModel.UrlPicturePath,
            Status = formModel.Status,
            ProjectEndDate = DateTime.Parse(formModel.ProjectEndDate),
        };
        await this.dbContext.Projects.AddAsync(project);
        await this.dbContext.SaveChangesAsync();
    }


    public async Task<bool> IsManagerOfProjectAsync(string userId, string managerId)
    {
        Project project = await this.dbContext
            .Projects
            .Where(x => x.IsActive)
            .FirstAsync(x => x.Id.ToString() == userId);
        return project.ManagerId.ToString() == managerId;
    }


    public async Task<AddAndEditProjectFormModel> GetProjectForEditByIdAsync(string projectId)
    {
        Project project = await this.dbContext
            .Projects
            .Include(x => x.Client)
            .Include(x => x.Manager)
            .Where(x => x.IsActive)
            .FirstAsync(x => x.Id.ToString() == projectId);


        var result = new AddAndEditProjectFormModel
        {
            Name = project.Name,
            Description = project.Description,
            ClientId = project.ClientId,
            ManagerId = project.ManagerId,
            UrlPicturePath = project.UrlPicturePath,
            Status = project.Status,
            ProjectEndDate = project.ProjectEndDate.ToString("dd/MM/yyyy"),
        };
        return result;
    }


    public async Task EditProjectByIdAsync(string projectId, AddAndEditProjectFormModel formModel)
    {
        Project project = await this.dbContext
            .Projects
            .Where(x => x.IsActive)
            .FirstAsync(x => x.Id.ToString() == projectId);

        project.Name = formModel.Name;
        project.Description = formModel.Description;
        project.ClientId = formModel.ClientId;
        project.ManagerId = formModel.ManagerId;
        project.UrlPicturePath = formModel.UrlPicturePath;
        project.Status = formModel.Status;
        project.ProjectEndDate =
            DateTime.ParseExact(formModel.ProjectEndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);


        await this.dbContext.SaveChangesAsync();
    }

    public async Task<bool> ProjectExistsByIdAsync(string id)
    {
        bool result = await this.dbContext
            .Projects
            .AnyAsync(x => x.Id.ToString() == id);
        return result;
    }
}