namespace CivilEngineerCMS.Services.Data;

using CivilEngineerCMS.Data;

using Interfaces;

using Microsoft.EntityFrameworkCore;

using Web.ViewModels.Home;

public class HomeService : IHomeService
{
    private readonly CivilEngineerCmsDbContext dbContext;

    public HomeService(CivilEngineerCmsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<IndexViewModel>> AllIndexProjectsAsync()
    {
        IEnumerable<IndexViewModel> allProjects = await this.dbContext
            .Projects
            .Where(p => p.UrlPicturePath != null)
            .OrderBy(x => x.ProjectCreatedDate)
            .Select(p => new IndexViewModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                UrlPicturePath = p.UrlPicturePath,
            })
            .Take(5)
            .ToListAsync();
        return allProjects;
    }
}