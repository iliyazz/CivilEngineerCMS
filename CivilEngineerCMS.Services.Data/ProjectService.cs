namespace CivilEngineerCMS.Services.Data;

using CivilEngineerCMS.Data;

using Interfaces;

using Microsoft.EntityFrameworkCore;

using Web.ViewModels.Project;

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
            .AnyAsync(x => x.UserId.ToString() == id);
        return managerExists;
    }
}

