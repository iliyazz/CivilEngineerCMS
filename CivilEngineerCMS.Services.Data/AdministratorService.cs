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

        public AdministratorService(CivilEngineerCmsDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        /// <summary>
        /// This method return all employees for choosing administrator
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// This method save all employees for administrator
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
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