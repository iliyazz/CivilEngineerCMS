namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using CivilEngineerCMS.Data;

    using Common;

    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public async Task<IActionResult> DeleteImage(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }


            //var project = await this.projectService.GetProjectForEditByIdAsync(id);





            string userId = this.User.GetId();
            bool isEmployee = await this.employeeService.IsEmployeeAsync(userId);
            bool isManagerInProject = false;
            if (isEmployee)
            {
                string employeeId = await employeeService.GetEmployeeIdByUserIdAsync(userId);
                string managerId = await projectService.GetManagerIdByProjectIdAsync(id);
                isManagerInProject = employeeId == managerId;
            }

            if (!(this.User.IsAdministrator() || isManagerInProject))
            {
                this.TempData[ErrorMessage] = "You must be manager of project you want to edit.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {
                var currentProject = await this.projectService.GetProjectByIdAsync(id);
                currentProject.ImageContent = null;
                currentProject.ImageName = null;
                currentProject.ContentType = null;
                if (currentProject.UrlPicturePath != null)
                {
                    await this.cloudinaryService.DeletePhotoAsync(currentProject.UrlPicturePath);
                }

                //dbContext.Update(currentProject);
                await dbContext.SaveChangesAsync();


                //if (!string.IsNullOrEmpty(projectImageName) || !string.IsNullOrWhiteSpace(projectImageContentType) || projectImage != null)
                //{
                //    var file = File(projectImage, projectImageContentType, projectImageName);
                //    return file;
                //}
                return this.RedirectToAction("Details", "Project", new { id });


            }
            catch (Exception e)
            {
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the project. Please try again later or contact administrator!";
                return GeneralError();

            }
        }
        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}