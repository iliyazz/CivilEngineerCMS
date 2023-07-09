﻿namespace CivilEngineerCMS.Services.Data
{
    using System.Security.Claims;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;
    using CivilEngineerCMS.Web.ViewModels.Manager;

    using Interfaces;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Employee;
    using Task = System.Threading.Tasks.Task;


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

        public async Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByManagerIdAsync(string id)
        {
            IEnumerable<MineManagerProjectViewModel> allProjectsByManagerIdAsync = await this.dbContext
                .Projects
                .Where(p => p.Manager.Id.ToString() == id)
                .OrderBy(p => p.Name)
                .Select(p => new MineManagerProjectViewModel
                {
                    Id = p.Id,
                    ProjectName = p.Name,
                    ClientName = p.Client.FirstName + " " + p.Client.LastName,
                    ClientPhoneNumber = p.Client.PhoneNumber,
                    ProjectCreatedDate = p.ProjectCreatedDate,
                    ProjectEndDate = p.ProjectEndDate,
                })
                .ToListAsync();
            return allProjectsByManagerIdAsync;
        }

        public string GetCurrentUserId(ClaimsPrincipal user)
        {
            string userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
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

        public async Task<string> GetManagerIdByUserIdAsync(string userId)
        {
            Employee? manager = await this.dbContext
                .Employees
                .FirstOrDefaultAsync(e => e.UserId.ToString() == userId);
            if (manager == null)
            {
                return null;
            }
            return manager.Id.ToString();
        }

        public async Task CreateEmployeeAsync(CreateEmployeeFormModel formModel)
        {

            Employee employee = new Employee
            {
                UserId = formModel.UserId,
                FirstName = formModel.FirstName,
                LastName = formModel.LastName,
                JobTitle = formModel.JobTitle,
                PhoneNumber = formModel.PhoneNumber,
                Address = formModel.Address,
            };
            await this.dbContext.Employees.AddAsync(employee);
            await this.dbContext.SaveChangesAsync();
        }

    }
}

