namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Employee;

    public class EmployeeService : IEmployeeService
    {
        private readonly CivilEngineerCmsDbContext dbContext;


        public EmployeeService(CivilEngineerCmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> EmployeeExistsByUserIdAsync(string id)
        {
            bool result = await this.dbContext.Employees.AnyAsync(e => e.UserId.ToString() == id);
            return result;
        }

        public async Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesAsync()
        {
            IEnumerable<AllEmployeeViewModel> allEmployeeAsync = await this.dbContext
                .Employees
                .OrderBy(x => x.JobTitle)
                .Select(e => new AllEmployeeViewModel
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle
                })
                .ToListAsync();
            return allEmployeeAsync;
        }

        public async Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByManagerIdAsync(string userId)
        {
            IEnumerable<MineManagerProjectViewModel> allProjectsByManagerIdAsync = await this.dbContext
                .Projects
                .Where(p => p.Manager.UserId.ToString() == userId)
                .OrderBy(pn => pn.Name)
                .Select(p => new MineManagerProjectViewModel
                {
                    ProjectName = p.Name,
                    ClientName = p.Client.FirstName + " " + p.Client.LastName,
                    ClientPhoneNumber = p.Client.PhoneNumber,
                    ProjectCreatedDate = p.ProjectCreatedDate,
                    ProjectEndDate = p.ProjectEndDate,
                    
                })
                .ToListAsync();
            return allProjectsByManagerIdAsync;
        }
    }
}



