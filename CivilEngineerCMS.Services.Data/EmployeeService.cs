﻿namespace CivilEngineerCMS.Services.Data
{
    using System.Security.Claims;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;
    using CivilEngineerCMS.Web.ViewModels.Manager;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Client;
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
            bool result = await this.dbContext.Employees.AnyAsync(e => e.Id.ToString() == id && e.IsActive);
            return result;
        }

        public async Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesAsync()
        {
            IEnumerable<AllEmployeeViewModel> allEmployeeAsync = await this.dbContext
                .Employees
                .Where(e => e.IsActive)
                .OrderBy(x => x.JobTitle)
                .Select(e => new AllEmployeeViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Email = e.User.Email,
                    PhoneNumber = e.PhoneNumber,
                })
                .ToListAsync();
            return allEmployeeAsync;
        }

        public async Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByManagerIdAsync(string id)
        {
            IEnumerable<MineManagerProjectViewModel> allProjectsByManagerIdAsync = await this.dbContext
                .Projects
                .Where(p => p.Manager.Id.ToString() == id && p.IsActive)
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

        public async Task<IEnumerable<ProjectSelectManagerFormModel>> AllManagersAsync()
        {
            IEnumerable<ProjectSelectManagerFormModel> managers = await this.dbContext
                .Employees
                .Where(e => e.IsActive)
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
                .Where(e => e.IsActive)
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

        public async Task<DetailsEmployeeViewModel> DetailsEmployeeAsync(string employeeId)
        {
            DetailsEmployeeViewModel employee = await this.dbContext
                .Employees
                .Include(e => e.User)
                .Where(e => e.Id.ToString() == employeeId && e.IsActive)
                .Select(e => new DetailsEmployeeViewModel()
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.User.Email,
                    Address = e.Address
                })
                .FirstAsync();

            var result = new DetailsEmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                JobTitle = employee.JobTitle,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Address = employee.Address
            };
            return result;
        }


        public async Task<EditEmployeeFormModel> GetEmployeeForEditByIdAsync(string employeeId)
        {
            Employee employee = await this.dbContext     
                .Employees
                .Include(e => e.User)
                .Where(e => e.Id.ToString() == employeeId && e.IsActive)
                .FirstAsync();
            string email = employee.User.Email;
            var result = new EditEmployeeFormModel
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                JobTitle = employee.JobTitle,
                Email = email
            };
            return result;
        }

        public async Task EditEmployeeByIdAsync(string employeeId, EditEmployeeFormModel formModel)
        {
            Employee employee = await this.dbContext
                .Employees
                .Include(e => e.User)
                .Where(e => e.Id.ToString() == employeeId && e.IsActive)
                .FirstAsync();
            employee.FirstName = formModel.FirstName;
            employee.LastName = formModel.LastName;
            employee.PhoneNumber = formModel.PhoneNumber;
            employee.Address = formModel.Address;
            employee.JobTitle = formModel.JobTitle;
            employee.User.Email = formModel.Email;

            await this.dbContext.SaveChangesAsync();
        }

    }
}
