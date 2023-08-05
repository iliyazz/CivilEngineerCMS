namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using Common;

    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using Services.Data.Models.Project;

    using ViewModels.Employee;
    using ViewModels.Project;

    using static Common.NotificationMessagesConstants;

    public class ProjectController : BaseAdminController
    {
        private readonly IProjectService projectService;
        private readonly IClientService clientService;
        private readonly IEmployeeService employeeService;

        public ProjectController(IProjectService projectService, IEmployeeService employeeService,
            IClientService clientService)
        {
            this.projectService = projectService;
            this.clientService = clientService;
            this.employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] ProjectAllQueryModel queryModel)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            ProjectAllFilteredAndPagedServiceModel serviceModel =
                await this.projectService.ProjectAllFilteredAndPagedAsync(queryModel);
            queryModel.Projects = serviceModel.Projects;
            queryModel.TotalProjects = serviceModel.TotalProjectsCount;
            queryModel.Statuses = Enum.GetNames(typeof(ProjectStatusEnums)).ToList();

            return this.View(queryModel);
        }

    }
}