namespace CivilEngineerCMS.Services.Data;

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

    public ProjectService( CivilEngineerCmsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<AllProjectViewModel>> AllProjectsAsync()
    {
        IEnumerable<AllProjectViewModel> allProjects = await this.dbContext
            .Projects
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

    public bool  StatusExists(string id)
    {
        bool statusExist = Enum.IsDefined(typeof(ProjectStatusEnums), id);
        return statusExist;
    }

    public async Task CreateProjectAsync(AddProjectFormModel formModel)
    {
        Project project = new Project
        {
            Name = formModel.Name,
            Description = formModel.Description,
            ClientId = formModel.ClientId,
            ManagerId = formModel.ManagerId,
            UrlPicturePath = formModel.UrlPicturePath,
            Status = formModel.Status,
            ProjectEndDate = formModel.ProjectEndDate,
        };
        await this.dbContext.Projects.AddAsync(project);
        await  this.dbContext.SaveChangesAsync();
    }
}

