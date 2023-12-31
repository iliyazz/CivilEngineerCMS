﻿using Microsoft.Extensions.Caching.Memory;

namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;
    using CivilEngineerCMS.Web.ViewModels.Manager;

    using Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Web.ViewModels.Employee;

    using Task = System.Threading.Tasks.Task;


    public class EmployeeService : IEmployeeService
    {
        private readonly CivilEngineerCmsDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        //private readonly IMemoryCache memoryCache;

        public EmployeeService(CivilEngineerCmsDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IUserService userService/*,
            IMemoryCache memoryCache*/)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.userService = userService;
            //this.memoryCache = memoryCache;
        }
        /// <summary>
        /// This method check if employee exists by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> EmployeeExistsByIdAsync(string id)
        {
            bool result = await this.dbContext.Employees.AnyAsync(e => e.Id.ToString() == id && e.IsActive);
            return result;
        }
        /// <summary>
        /// This method return all employees for administrator area
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesAsync()
        {
            IEnumerable<AllEmployeeViewModel> allEmployeeAsync = await this.dbContext
                .Employees
                //.Where(e => e.IsActive)
                .OrderBy(x => x.JobTitle)
                .Select(e => new AllEmployeeViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Email = e.User.Email,
                    PhoneNumber = e.PhoneNumber,
                    IsActive = e.IsActive,
                    UserId = e.UserId
                })
                .ToListAsync();
            return allEmployeeAsync;
        }
        /// <summary>
        /// This method return all projects depending on manager id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This method return all employees and managers for project
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectEmployeesAndManagerForProjectFormModel>> AllEmployeesAndManagersAsync()
        {
            IEnumerable<SelectEmployeesAndManagerForProjectFormModel> managers = await this.dbContext
                .Employees
                .Where(e => e.IsActive)
                .Select(u => new SelectEmployeesAndManagerForProjectFormModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    JobTitle = u.JobTitle
                })
                .ToListAsync();
            return managers;
        }
        /// <summary>
        /// This method return manager id by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This method create employee
        /// </summary>
        /// <param name="formModel"></param>
        /// <returns></returns>
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

            await this.userService.AddClaimToUserAsync(employee.UserId.ToString(), "FullName",
                $"{employee.FirstName} {employee.LastName}");
        }
        /// <summary>
        /// This method return details for employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This method return employee for edit
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This method edit employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This method return employee for delete
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<EmployeePreDeleteViewModel> GetEmployeeForPreDeleteByIdAsync(string employeeId)
        {
            Employee employee = await this.dbContext
                .Employees
                .Include(c => c.User)
                .Where(c => c.IsActive)
                .FirstAsync(c => c.Id.ToString() == employeeId);
            return new EmployeePreDeleteViewModel
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                JobTitle = employee.JobTitle,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                Email = employee.User.Email
            };
        }
        /// <summary>
        /// This method delete employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task DeleteEmployeeByIdAsync(string employeeId)
        {
            Employee employeeToDelete = await this.dbContext
                .Employees
                .Where(c => c.IsActive)
                .Include(c => c.User)
                .FirstAsync(c => c.Id.ToString() == employeeId);
            employeeToDelete.IsActive = false;

            Guid userId = employeeToDelete.User.Id;
            var userToDelete = await this.userManager.FindByIdAsync(userId.ToString());

            if (userToDelete != null)
            {
                await this.userManager.SetLockoutEnabledAsync(userToDelete, true);
                await this.userManager.SetLockoutEndDateAsync(userToDelete,
                    new System.DateTimeOffset(System.DateTime.Now.AddYears(100)));
            }

            await this.dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method return all employees working on project with id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AllEmployeeViewModel>> AllEmployeesByProjectIdAsync(string projectId)
        {
            IEnumerable<AllEmployeeViewModel> allEmployeesByProjectIdAsync = await this.dbContext
                .ProjectsEmployees
                .Include(e => e.Employee)
                .Where(e => e.ProjectId.ToString() == projectId)
                .Select(e => new AllEmployeeViewModel
                {
                    Id = e.Employee.Id,
                    FirstName = e.Employee.FirstName,
                    LastName = e.Employee.LastName,
                    JobTitle = e.Employee.JobTitle,
                    Email = e.Employee.User.Email,
                    PhoneNumber = e.Employee.PhoneNumber,
                })
                .ToListAsync();
            return allEmployeesByProjectIdAsync;
        }
        /// <summary>
        /// This method check if employee is working on project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<bool> IsEmployeeInProjectAsync(string projectId, string employeeId)
        {
            bool isEmployeeInProject = await this.dbContext
                .ProjectsEmployees
                .AnyAsync(x => x.ProjectId.ToString() == projectId && x.EmployeeId.ToString() == employeeId);
            return isEmployeeInProject;
        }
        /// <summary>
        /// This method check if user is employee depending on userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsEmployeeAsync(string userId)
        {
            return await this.dbContext
                .Employees
                .Where(e => e.IsActive)
                .AnyAsync(e => e.UserId.ToString() == userId);

        }
        /// <summary>
        /// This method return employeeId depending on userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetEmployeeIdByUserIdAsync(string userId)
        {
            return await this.dbContext
                .Employees
                .Where(e => e.IsActive && e.UserId.ToString() == userId)
                .Select(e => e.Id.ToString())
                .FirstAsync();
        }
        /// <summary>
        /// This method return all projects for employee with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MineManagerProjectViewModel>> AllProjectsByEmployeeIdAsync(string id)
        {
            IEnumerable<MineManagerProjectViewModel> allProjectsByManagerIdAsync = await this.dbContext
                .Projects
                .Include(p => p.ProjectsEmployees)
                .Where(p => p.IsActive && p.ProjectsEmployees.Any(pe => pe.EmployeeId.ToString() == id))
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

        
    }
}