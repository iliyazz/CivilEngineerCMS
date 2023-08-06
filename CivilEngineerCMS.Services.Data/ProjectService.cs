namespace CivilEngineerCMS.Services.Data
{
    using System.Globalization;

    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;

    using Common;

    using Interfaces;

    using Microsoft.EntityFrameworkCore;

    using Models.Project;
    using Models.Statistics;

    using Web.ViewModels.Employee;
    using Web.ViewModels.Project;
    using Web.ViewModels.Project.Enums;

    using Task = System.Threading.Tasks.Task;

    public class ProjectService : IProjectService
    {
        private readonly CivilEngineerCmsDbContext dbContext;

        public ProjectService(CivilEngineerCmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool StatusExists(string id)
        {
            bool statusExist = Enum.IsDefined(typeof(ProjectStatusEnums), id);
            return statusExist;
        }
        /// <summary>
        /// This method create project
        /// </summary>
        /// <param name="formModel"></param>
        /// <returns></returns>
        public async Task CreateProjectAsync(AddAndEditProjectFormModel formModel)
        {
            Project project = new Project
            {
                Name = formModel.Name,
                Description = formModel.Description,
                ClientId = formModel.ClientId,
                ManagerId = formModel.ManagerId,
                UrlPicturePath = formModel.UrlPicturePath,
                Status = formModel.Status,
                ProjectEndDate = DateTime.Parse(formModel.ProjectEndDate),
            };
            await this.dbContext.Projects.AddAsync(project);
            await this.dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method check if employee is manager of project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public async Task<bool> IsManagerOfProjectAsync(string projectId, string managerId)
        {
            Project project = await this.dbContext
                .Projects
                .Where(x => x.IsActive)
                .FirstAsync(x => x.Id.ToString() == projectId);
            bool isManagerOfProject = string.Equals(project.ManagerId.ToString(), managerId,
                StringComparison.CurrentCultureIgnoreCase);
            return isManagerOfProject;
        }
        /// <summary>
        /// This method return project for edit by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AddAndEditProjectFormModel> GetProjectForEditByIdAsync(string id)
        {
            Project project = await this.dbContext
                .Projects
                .Include(x => x.Client)
                .Include(x => x.Manager)
                .Include(x => x.ProjectsEmployees)
                .ThenInclude(x => x.Employee)
                .Where(x => x.IsActive)
                .FirstAsync(x => x.Id.ToString() == id);


            var result = new AddAndEditProjectFormModel
            {
                Name = project.Name,
                Description = project.Description,
                ClientId = project.ClientId,
                ManagerId = project.ManagerId,
                UrlPicturePath = project.UrlPicturePath,
                Status = project.Status,
                ProjectEndDate = project.ProjectEndDate.ToString("dd/MM/yyyy"),

                Employees = project.ProjectsEmployees.Where(pe => pe.ProjectId.ToString() == id).Select(t =>
                    new AllEmployeeViewModel
                    {
                        Id = t.Employee.Id,
                        FirstName = t.Employee.FirstName,
                        LastName = t.Employee.LastName,
                        //Email = t.Employee.User.Email,
                        JobTitle = t.Employee.JobTitle,
                        PhoneNumber = t.Employee.PhoneNumber,
                    }).ToList()
            };
            return result;
        }
        /// <summary>
        /// This method edit project by id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
        public async Task EditProjectByIdAsync(string projectId, AddAndEditProjectFormModel formModel)
        {
            Project project = await this.dbContext
                .Projects
                .Include(x => x.Manager)
                .Include(x => x.ProjectsEmployees)
                .Where(x => x.IsActive)
                .FirstAsync(x => x.Id.ToString() == projectId);

            project.Name = formModel.Name;
            project.Description = formModel.Description;
            project.ClientId = formModel.ClientId;
            project.ManagerId = formModel.ManagerId;
            project.UrlPicturePath = formModel.UrlPicturePath;
            project.Status = formModel.Status;
            project.ProjectEndDate =
                DateTime.ParseExact(formModel.ProjectEndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            await this.dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method return details project by id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<DetailsProjectViewModel> DetailsByIdProjectAsync(string projectId)
        {
            DetailsProjectViewModel project = await this.dbContext
                .Projects
                .Include(x => x.Client)
                .Include(x => x.Manager)
                .Include(x => x.ProjectsEmployees)
                .ThenInclude(x => x.Employee)
                .Where(x => x.IsActive && x.Id.ToString() == projectId)
                .Select(x => new DetailsProjectViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ClientName = x.Client.FirstName + " " + x.Client.LastName,
                    ClientPhone = x.Client.PhoneNumber,
                    ClientEmail = x.Client.User.Email,
                    ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                    ProjectStartDate = x.ProjectCreatedDate.ToString("dd.MM.yyyy"),
                    ProjectEndDate = x.ProjectEndDate.ToString("dd.MM.yyyy"),
                    Status = x.Status,
                    Employees = x.ProjectsEmployees.Where(p => p.ProjectId.ToString() == projectId).Select(pe =>
                        new DetailsEmployeeViewModel
                        {
                            Id = pe.Employee.Id,
                            FirstName = pe.Employee.FirstName,
                            LastName = pe.Employee.LastName,
                            PhoneNumber = pe.Employee.PhoneNumber,
                            Email = pe.Employee.User.Email,
                            Address = pe.Employee.Address,
                            JobTitle = pe.Employee.JobTitle,
                        }
                    ).ToList()
                })
                .FirstAsync();
            var result = new DetailsProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ClientName = project.ClientName,
                ClientPhone = project.ClientPhone,
                ClientEmail = project.ClientEmail,
                ManagerName = project.ManagerName,
                ProjectStartDate = project.ProjectStartDate,
                ProjectEndDate = project.ProjectEndDate,
                Status = project.Status,
                Employees = project.Employees
            };
            return result;
        }
        /// <summary>
        /// This method check if project exists by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ProjectExistsByIdAsync(string id)
        {
            bool result = await this.dbContext
                .Projects
                .Where(x => x.IsActive)
                .AnyAsync(x => x.Id.ToString() == id);
            return result;
        }
        /// <summary>
        /// This method return project for delete by id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ProjectPreDeleteViewModel> GetProjectForPreDeleteByIdAsync(string projectId)
        {
            ProjectPreDeleteViewModel projectToDelete = await this.dbContext
                .Projects
                .Include(x => x.Client)
                .Include(x => x.Manager)
                .Where(x => x.IsActive && x.Id.ToString() == projectId)
                .Select(p => new ProjectPreDeleteViewModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    ClientName = p.Client.FirstName + " " + p.Client.LastName,
                    ManagerName = p.Manager.FirstName + " " + p.Manager.LastName,
                    ProjectStartDate = p.ProjectCreatedDate.ToString("dd.MM.yyyy"),
                    ProjectEndDate = p.ProjectEndDate.ToString("dd.MM.yyyy"),
                    Status = p.Status
                })
                .FirstAsync();
            return projectToDelete;
        }
        /// <summary>
        /// This method delete project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProjectByIdAsync(string id)
        {
            Project projectToDelete = await this.dbContext
                .Projects
                .Where(x => x.IsActive)
                .FirstAsync(x => x.Id.ToString() == id);
            projectToDelete.IsActive = false;
            await this.dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method return all projects in IQueryable for sorting and filtering
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async Task<ProjectAllFilteredAndPagedServiceModel> ProjectAllFilteredAndPagedAsync(
            ProjectAllQueryModel queryModel)
        {
            IQueryable<Project> projectsQuery = this.dbContext
                .Projects
                .Where(x => x.IsActive)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(queryModel.Status.ToString()))
            {
                projectsQuery = projectsQuery
                    .Where(p => p.Status == queryModel.Status);
            }


            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";
                projectsQuery = projectsQuery
                    .Where(p => EF.Functions.Like(p.Name, wildCard) ||
                                EF.Functions.Like(p.Description, wildCard) ||
                                EF.Functions.Like(p.Client.User.Email, wildCard) ||
                                EF.Functions.Like(p.Manager.User.Email, wildCard) ||
                                EF.Functions.Like(p.Client.PhoneNumber, wildCard) ||
                                EF.Functions.Like(p.Manager.PhoneNumber, wildCard) ||
                                EF.Functions.Like(p.Client.FirstName, wildCard) ||
                                EF.Functions.Like(p.Client.LastName, wildCard) ||
                                EF.Functions.Like(p.Manager.FirstName, wildCard) ||
                                EF.Functions.Like(p.Manager.LastName, wildCard) ||
                                EF.Functions.Like(p.Manager.JobTitle, wildCard));
            }


            projectsQuery = queryModel.ProjectSorting switch
            {
                ProjectSorting.ProjectName => projectsQuery.OrderBy(p => p.Name),
                ProjectSorting.Status => projectsQuery.OrderBy(p => p.Status),
                ProjectSorting.Description => projectsQuery.OrderBy(p => p.Description),
                ProjectSorting.ClientName => projectsQuery.OrderBy(p => p.Client.FirstName)
                    .ThenBy(p => p.Client.LastName),
                ProjectSorting.ClientEmail => projectsQuery.OrderBy(p => p.Client.User.Email),
                ProjectSorting.ClientPhone => projectsQuery.OrderBy(p => p.Client.PhoneNumber),
                ProjectSorting.ManagerName => projectsQuery.OrderBy(p => p.Manager.FirstName)
                    .ThenBy(p => p.Manager.LastName),
                ProjectSorting.ManagerEmail => projectsQuery.OrderBy(p => p.Manager.User.Email),
                ProjectSorting.ManagerPhone => projectsQuery.OrderBy(p => p.Manager.PhoneNumber),

                _ => projectsQuery.OrderBy(p => p.ProjectCreatedDate)
                    .ThenBy(p => p.Status)
            };
            IEnumerable<ProjectAllViewModel> allProjects = await projectsQuery
                .Where(p => p.IsActive)
                .Skip((queryModel.CurrentPage - 1) * queryModel.ProjectsPerPage)
                .Take(queryModel.ProjectsPerPage)
                .Select(p => new ProjectAllViewModel
                {
                    Id = p.Id.ToString(),
                    ProjectName = p.Name,
                    Description = p.Description,
                    ManagerName = $"{p.Manager.FirstName} {p.Manager.LastName}",
                    ClientEmail = p.Client.User.Email,
                    ManagerPhoneNumber = p.Manager.PhoneNumber,
                    ManagerEmail = p.Manager.User.Email,
                    ProjectCreatedDate = p.ProjectCreatedDate,
                    ProjectEndDate = p.ProjectEndDate,
                    ClientName = $"{p.Client.FirstName} {p.Client.LastName}",
                    Status = p.Status.ToString(),
                    ClientPhoneNumber = p.Client.PhoneNumber
                })
                .ToArrayAsync();
            int totalProjects = await projectsQuery.CountAsync();
            ProjectAllFilteredAndPagedServiceModel result = new ProjectAllFilteredAndPagedServiceModel
            {
                TotalProjectsCount = totalProjects,
                Projects = allProjects
            };
            return result;
        }
        /// <summary>
        /// This method return statistics for projects and clients
        /// </summary>
        /// <returns></returns>
        public async Task<StatisticsServiceModel> GetStatisticsAsync()
        {
            var result =  new StatisticsServiceModel()
            {
                TotalActiveProjects = await this.dbContext
                    .Projects
                    .Where(x => x.IsActive)
                    .CountAsync(),
                TotalProjects = await this.dbContext
                    .Projects
                    .CountAsync(),
                TotalClients = await this.dbContext
                    .Clients
                    .CountAsync(),
            };
            return result;
        }
        /// <summary>
        /// This method return all employees for project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectEmployeesForProjectFormModel>> AllEmployeesForProjectAsync(string projectId)
        {
            IEnumerable<SelectEmployeesForProjectFormModel> allEmployees = await this.dbContext
                .Employees
                .Where(x => x.IsActive)
                .Include(x => x.Projects)
                .Include(x => x.ProjectsEmployees)
                .Select(e => new SelectEmployeesForProjectFormModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.User.Email,
                    IsChecked = e.ProjectsEmployees.Any(pe => pe.EmployeeId == e.Id && pe.ProjectId.ToString() == projectId)
                })
                .ToListAsync();

            return allEmployees;
        }
        /// <summary>
        /// This method save all employees for project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task SaveAllEmployeesForProjectAsync(string projectId, IEnumerable<string> idList)
        {
            Project project = await this.dbContext
                .Projects
                .Where(x => x.IsActive)
                .Include(x => x.ProjectsEmployees)
                .ThenInclude(x => x.Employee)
                .FirstAsync(x => x.Id.ToString() == projectId);

            project.ProjectsEmployees.Clear();

            foreach (var id in idList)
            {
                project.ProjectsEmployees.Add(new ProjectEmployee
                {
                    EmployeeId = Guid.Parse(id),
                    ProjectId = Guid.Parse(projectId)
                });
            }

            await this.dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method return project by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<Project> GetProjectByIdAsync(string projectId)
        {
            Project project = await this.dbContext
                .Projects
                .Where(p => p.IsActive && p.Id.ToString() == projectId)
                .FirstAsync();
            return project;
        }
        /// <summary>
        /// This method return manager id by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<string> GetManagerIdByProjectIdAsync(string projectId)
        {
            return await this.dbContext
                .Projects
                .Where(p => p.IsActive && p.Id.ToString() == projectId)
                .Select(p => p.ManagerId.ToString())
                .FirstAsync();
        }
        /// <summary>
        /// This method check if employee is employee of project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<bool> IsEmployeeOfProjectAsync(string projectId, string employeeId)
        {
            var isEmployeeOfProject = await this.dbContext
                .ProjectsEmployees
                .AnyAsync(pe => pe.ProjectId.ToString() == projectId && pe.EmployeeId.ToString() == employeeId);
            return isEmployeeOfProject;
        }
    }
}