namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Manager;

    public class ManagerService : IManagerService
    {
        private readonly CivilEngineerCmsDbContext dbContext;

        public ManagerService(CivilEngineerCmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<ProjectSelectManagerFormModel>> AllManagersAsync()
        {
            IEnumerable<ProjectSelectManagerFormModel> managers = await this.dbContext
                .Employees
                .Select(u => new ProjectSelectManagerFormModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    JobTitle = u.JobTitle
                })
                .ToListAsync();
            return managers;
        }
    }
}
