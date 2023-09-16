namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using CivilEngineerCMS.Data;

    using Common;

    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using Services.Data.Models.Project;

    using ViewModels.Project;

    using static Common.NotificationMessagesConstants;

    public class ProjectController : BaseAdminController
    {
        private readonly IProjectService projectService;
        private readonly IClientService clientService;
        private readonly IEmployeeService employeeService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly CivilEngineerCmsDbContext dbContext;

        public ProjectController(IProjectService projectService, IEmployeeService employeeService,
            IClientService clientService, ICloudinaryService cloudinaryService, CivilEngineerCmsDbContext dbContext)
        {
            this.projectService = projectService;
            this.clientService = clientService;
            this.employeeService = employeeService;
            this.cloudinaryService = cloudinaryService;
            this.dbContext = dbContext;
        }
        /// <summary>
        /// This method return view for all projects
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
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

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}