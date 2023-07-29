namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;

    using Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Web.ViewModels.Administrator;

    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    public class AdministratorService : IAdministratorService
    {
        private readonly CivilEngineerCmsDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public AdministratorService(CivilEngineerCmsDbContext dbContext, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        public async Task AssignAdministratorRoleAsync(string userId)
        {
            ApplicationUser? user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            IdentityRole<Guid>? role =
                await this.dbContext.Roles.FirstOrDefaultAsync(r => r.Name == AdministratorRoleName);
            if (user != null && role != null)
            {
                await this.userManager.AddToRoleAsync(user, role.Name);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveAdministratorRoleAsync(string userId)
        {
            ApplicationUser? user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            IdentityRole<Guid>? role =
                await this.dbContext.Roles.FirstOrDefaultAsync(r => r.Name == AdministratorRoleName);
            if (user != null && role != null)
            {
                await this.userManager.RemoveFromRoleAsync(user, role.Name);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SelectEmployeesForAdministratorFormModel>> AllEmployeesForAdministratorAsync()
        {
            IEnumerable<SelectEmployeesForAdministratorFormModel> allEmployees = await this.dbContext
                .Employees
                .Where(x => x.IsActive)
                .Include(x => x.User)
                .Select(e => new SelectEmployeesForAdministratorFormModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Email = e.User.Email,
                    IsChecked = this.userManager.IsInRoleAsync(e.User, AdministratorRoleName).GetAwaiter().GetResult(),
                    UserId = e.UserId
                })
                .ToListAsync();


            return allEmployees;
        }

        public async Task SaveAllEmployeesForAdministratorAsync(string currentUserId, IEnumerable<string> idList)
        {
            var enumerable = idList.ToList();

            IEnumerable<SelectEmployeesForAdministratorFormModel> allEmployees =
                await this.AllEmployeesForAdministratorAsync();
            if (enumerable.Any())
                foreach (var employee in allEmployees)
                {
                    var user = await this.userManager.FindByEmailAsync(employee.Email);
                    if (enumerable.Any(s => s == employee.UserId.ToString()))
                    {
                        await userManager.AddToRoleAsync(user, AdministratorRoleName);
                    }
                    else
                    {
                        await userManager.RemoveFromRoleAsync(user, AdministratorRoleName);
                    }
                }
        }
    }
}