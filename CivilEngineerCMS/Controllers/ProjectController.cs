using CivilEngineerCMS.Common;

using CloudinaryDotNet.Actions;

namespace CivilEngineerCMS.Web.Controllers
{
    using CivilEngineerCMS.Data;

    using Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    using Services.Data.Interfaces;

    using ViewModels.Employee;
    using ViewModels.Project;

    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;
        private readonly IClientService clientService;
        private readonly IEmployeeService employeeService;
        private readonly ILogger<ProjectController> logger;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly CivilEngineerCmsDbContext dbContext;
        private readonly ICloudinaryService cloudinaryService;

        public ProjectController(IProjectService projectService,
            IEmployeeService employeeService,
            IClientService clientService,
            ILogger<ProjectController> logger,
            IWebHostEnvironment hostingEnvironment,
            CivilEngineerCmsDbContext dbContext, ICloudinaryService cloudinaryService)
        {
            this.projectService = projectService;
            this.clientService = clientService;
            this.employeeService = employeeService;
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
            this.dbContext = dbContext;
            this.cloudinaryService = cloudinaryService;
        }

        /// <summary>
        /// Returns the form view for adding a new project.
        /// </summary>
        /// <returns>The add project form view pre-populated with available managers and clients, or a redirect to the home page if the user is not an administrator.</returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            AddAndEditProjectFormModel formModel = new AddAndEditProjectFormModel
            {
                Managers = await this.employeeService.AllEmployeesAndManagersAsync(),
                Clients = await this.clientService.AllClientsAsync()
            };

            return this.View(formModel);
        }

        /// <summary>
        /// Creates a new project from the submitted form data, including optional image upload.
        /// </summary>
        /// <param name="formModel">The form model containing project data and an optional image file.</param>
        /// <returns>Redirects to the project list on success, or returns the form view with validation errors on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddAndEditProjectFormModel formModel)
        {
            bool isAdministrator = this.User.IsAdministrator();
            if (!isAdministrator)
            {
                this.TempData[ErrorMessage] = "You are not authorized to view this page.";
                return this.RedirectToAction("Index", "Home");
            }

            bool managerExists = await this.employeeService.EmployeeExistsByIdAsync(formModel.ManagerId.ToString());

            if (!managerExists)
            {
                this.ModelState.AddModelError(nameof(formModel.ManagerId), "Manager does not exist.");
            }

            bool clientExists = await this.clientService.ClientExistsByIdAsync(formModel.ClientId.ToString());
            if (!clientExists)
            {
                this.ModelState.AddModelError(nameof(formModel.ClientId), "Client does not exist.");
            }

            bool statusExists = this.projectService.StatusExists(formModel.Status.ToString());
            if (!statusExists)
            {
                this.ModelState.AddModelError(nameof(formModel.Status), "Name does not exist.");
            }

            if (DateTime.UtcNow > DateTime.Parse(formModel.ProjectEndDate))
            {
                this.ModelState.AddModelError(nameof(formModel.ProjectEndDate),
                    "Project End Date cannot be before start date.");
            }

            if (formModel.ImageContent != null)
            {
                if (formModel.ImageContent.Length > GeneralApplicationConstants.ImageMaxSizeInBytes)
                {
                    this.ModelState.AddModelError(nameof(formModel.ImageContent),
                        NotificationMessagesConstants.ExceedingMaxSizeMessage);
                    this.TempData[ErrorMessage] = NotificationMessagesConstants.ExceedingMaxSizeMessage;
                }

                if (!projectService.IfFileIsImage(formModel.ImageContent))
                {
                    this.ModelState.AddModelError(nameof(formModel.ImageContent),
                        NotificationMessagesConstants.InvalidFileExtensionMessage);
                    this.TempData[ErrorMessage] = NotificationMessagesConstants.InvalidFileExtensionMessage;
                }

                if (!cloudinaryService.IsFileValid(formModel.ImageContent))
                {
                    this.ModelState.AddModelError(nameof(formModel.ImageContent),
                        NotificationMessagesConstants.InvalidFileExtensionMessage);
                    this.TempData[ErrorMessage] = NotificationMessagesConstants.InvalidFileExtensionMessage;
                }
            }





            if (!this.ModelState.IsValid)
            {

                formModel.Managers = await this.employeeService.AllEmployeesAndManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                return this.View(formModel);
            }

            try
            {

                if (formModel.ImageContent != null)
                {
                    ImageUploadResult uploadResult =
                        await this.cloudinaryService.UploadPhotoAsync(formModel.ImageContent, "CMS/projects", null);
                    formModel.PublicId = uploadResult.PublicId;
                    formModel.UrlPicturePath = uploadResult.Url.AbsolutePath;
                }

                await this.projectService.CreateProjectAsync(formModel);

                this.TempData[SuccessMessage] = $"Project {formModel.Name} added successfully.";
                return this.RedirectToAction("All", "Project", new { Area = AdminAreaName });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating project {ProjectName}.", formModel.Name);
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the project. Please try again later or contact administrator!");
                formModel.Managers = await this.employeeService.AllEmployeesAndManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                return this.View(formModel);
            }
        }

        /// <summary>
        /// Returns a view listing all employees assigned to the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project.</param>
        /// <returns>A view with the project's employee list, or a redirect if the project does not exist or the user is not authorized.</returns>
        [HttpGet]
        public async Task<IActionResult> AllEmployeesInProject(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }

            var project = await this.projectService.GetProjectForEditByIdAsync(id);
            var currentUserId = this.User.GetId();

            var projectManagerUserId = await this.employeeService.GetManagerIdByUserIdAsync(currentUserId);
            bool isAdministrator = this.User.IsAdministrator();

            if (!(this.User.IsAdministrator() || project.ManagerId.ToString() == projectManagerUserId))
            {
                this.TempData[ErrorMessage] = "You must be manager of project you want to edit.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {
                //var employees = await this.employeeService.AllEmployeesAndManagersAsync();
                IEnumerable<AllEmployeeViewModel> projectEmployees =
                    await this.employeeService.AllEmployeesByProjectIdAsync(id);

                return this.View(projectEmployees);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading employees for project {ProjectId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the project. Please try again later or contact administrator!";
                return this.RedirectToAction("Mine", "Employee");
            }
        }

        /// <summary>
        /// Returns the edit form view for the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project to edit.</param>
        /// <returns>The edit project form view, or a redirect if the project does not exist or the user is not authorized.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }


            var project = await this.projectService.GetProjectForEditByIdAsync(id);

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
                AddAndEditProjectFormModel formModel = await this.projectService.GetProjectForEditByIdAsync(id);
                formModel.Managers = await this.employeeService.AllEmployeesAndManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                formModel.Employees = await this.employeeService.AllEmployeesByProjectIdAsync(id);


                return this.View(formModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading employees for project {ProjectId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the project. Please try again later or contact administrator!";
                return this.RedirectToAction("Mine", "Employee");
            }
        }

        /// <summary>
        /// Processes the submitted edit form and updates the specified project, including optional image replacement.
        /// </summary>
        /// <param name="id">The unique identifier of the project to update.</param>
        /// <param name="formModel">The form model containing the updated project data and an optional new image file.</param>
        /// <returns>Redirects to the project list on success, or returns the form view with errors on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddAndEditProjectFormModel formModel)
        {
            if (formModel.ImageContent != null)
            {
                if (formModel.ImageContent.Length > GeneralApplicationConstants.ImageMaxSizeInBytes)
                {
                    this.ModelState.AddModelError(nameof(formModel.ImageContent),
                        NotificationMessagesConstants.ExceedingMaxSizeMessage);
                    this.TempData[ErrorMessage] = NotificationMessagesConstants.ExceedingMaxSizeMessage;
                }

                if (!projectService.IfFileIsImage(formModel.ImageContent))
                {
                    this.ModelState.AddModelError(nameof(formModel.ImageContent),
                        NotificationMessagesConstants.InvalidFileExtensionMessage);
                    this.TempData[ErrorMessage] = NotificationMessagesConstants.InvalidFileExtensionMessage;
                }
            }

            if (!this.ModelState.IsValid)
            {
                formModel.Managers = await this.employeeService.AllEmployeesAndManagersAsync();
                formModel.Clients = await this.clientService.AllClientsAsync();
                formModel.Employees = await this.employeeService.AllEmployeesByProjectIdAsync(id);

                return this.View(formModel);
            }


            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }


            var project = await this.projectService.GetProjectForEditByIdAsync(id);

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
                if(formModel.ImageContent == null && project.UrlPicturePath != null)
                {
                    formModel.ImageName = project.ImageName;
                    formModel.ContentType = project.ContentType;
                    formModel.UrlPicturePath = project.UrlPicturePath;
                    formModel.PublicId = project.PublicId;
                }
                else if (formModel.PublicId == null || formModel.ImageName != project.ImageName)
                    {
                        if (formModel.ImageContent != null)
                        {
                            ImageUploadResult uploadResult = await this.cloudinaryService.UploadPhotoAsync(formModel.ImageContent, "CMS/projects", project.PublicId);
                            formModel.PublicId = uploadResult.PublicId;
                            formModel.UrlPicturePath = uploadResult.Url.AbsoluteUri;
                        }
                    }
                //else
                //{
                //    formModel.ImageContent = project.ImageContent;
                //}



                await this.projectService.EditProjectByIdAsync(id, formModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error editing project {ProjectId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the project. Please try again later or contact administrator!";
                return this.View(formModel);
            }

            this.TempData[SuccessMessage] = $"Project {formModel.Name} edited successfully.";
            formModel.Managers = await this.employeeService.AllEmployeesAndManagersAsync();
            formModel.Clients = await this.clientService.AllClientsAsync();
            formModel.Employees = await this.employeeService.AllEmployeesByProjectIdAsync(id);

            if (this.User.IsAdministrator())
            {
                return this.RedirectToAction("All", "Project", new { Area = AdminAreaName });
            }

            return this.RedirectToAction("Mine", "Employee");
        }

        /// <summary>
        /// Returns the details view for the project identified by the route id value.
        /// </summary>
        /// <returns>The project details view, or a redirect if the project does not exist or the user is not authorized to view it.</returns>
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var projectId = (string?)Url.ActionContext.RouteData.Values["id"];
            if (projectId == null)
            {
                this.TempData[ErrorMessage] = "Project does not exist.";
                this.ModelState.AddModelError(string.Empty, "Project does not exist.");
                return this.RedirectToAction("All", "Project");
            }

            if (!await projectService.ProjectExistsByIdAsync(projectId))
            {
                this.TempData[ErrorMessage] = "Project does not exist.";
                this.ModelState.AddModelError(string.Empty, "Project does not exist.");
                return this.RedirectToAction("All", "Project");
            }
            var currentUserId = this.User.GetId();
            var isClient = await clientService.IsClientByUserIdAsync(currentUserId);
            var isAdministrator = this.User.IsAdministrator();

            if (!isAdministrator)
            {
                if (isClient)
                {
                    string clientIdByUserId = await this.clientService.GetClientIdByUserIdAsync(currentUserId);
                    string? clientIdByProjectId = await this.clientService.GetClientIdByProjectIdAsync(projectId);
                    bool isClientOfProject = clientIdByProjectId != null &&
                        string.Equals(clientIdByUserId, clientIdByProjectId, StringComparison.OrdinalIgnoreCase);
                    if (!isClientOfProject)
                    {
                        NotAuthorized();
                        return this.RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    try
                    {
                        string employeeId = await employeeService.GetEmployeeIdByUserIdAsync(currentUserId);
                        bool isEmployeeOfProject = await projectService.IsEmployeeOfProjectAsync(projectId, employeeId);
                        bool isManagerOfProject = await projectService.IsManagerOfProjectAsync(projectId, employeeId);

                        if (!(isEmployeeOfProject || isManagerOfProject))
                        {
                            NotAuthorized();
                            return this.RedirectToAction("Index", "Home");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Authorization check failed for project details. UserId: {UserId}, ProjectId: {ProjectId}", currentUserId, projectId);
                        NotAuthorized();
                        return this.RedirectToAction("Index", "Home");
                    }
                }
            }



            DetailsProjectViewModel viewModel = await this.projectService.DetailsByIdProjectAsync(projectId);
            return this.View(viewModel);
        }

        /// <summary>
        /// Sets an authorization error message in TempData and ModelState for the current request.
        /// </summary>
        private void NotAuthorized()
        {
            this.TempData[ErrorMessage] = "You are not authorized to view this page.";
            this.ModelState.AddModelError(string.Empty, "You are not authorized to view this page.");
        }


        /// <summary>
        /// Returns the delete confirmation view for the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project to delete.</param>
        /// <returns>The delete confirmation view, or a redirect if the project does not exist or the user is not authorized.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }


            var project = await this.projectService.GetProjectForEditByIdAsync(id);

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
                this.TempData[ErrorMessage] = "You must be manager of project you want to delete.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {
                ProjectPreDeleteViewModel viewModel = await this.projectService.GetProjectForPreDeleteByIdAsync(id);
                this.TempData[WarningMessage] = $"You are going to delete project {viewModel.Name}.";
                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading project {ProjectId} for delete.", id);
                return GeneralError();
            }
        }

        /// <summary>
        /// Deletes the specified project permanently.
        /// </summary>
        /// <param name="id">The unique identifier of the project to delete.</param>
        /// <param name="formModel">The pre-delete view model containing the project's display data.</param>
        /// <returns>Redirects to the project list on success, or to the employee projects page on unexpected error.</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id, ProjectPreDeleteViewModel formModel)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }


            var currentProject = await this.projectService.GetProjectForEditByIdAsync(id);

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
                this.TempData[ErrorMessage] = "You must be manager of project you want to delete.";
                return this.RedirectToAction("Mine", "Employee");
            }

            try
            {

                await this.projectService.DeleteProjectByIdAsync(id);

                this.TempData[WarningMessage] =
                    $"Project {formModel.Name} deleted successfully.";
                if (this.User.IsAdministrator())
                {
                    return this.RedirectToAction("All", "Project", new { Area = AdminAreaName });
                }

                return this.RedirectToAction("Mine", "Employee");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting project {ProjectId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while deleting the project. Please try again later or contact administrator!";
                return this.RedirectToAction("All", "Project");
            }
        }

        /// <summary>
        /// Sets a generic error message and redirects to the home page.
        /// </summary>
        /// <returns>A redirect result to the home page.</returns>
        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns the view for managing (selecting/deselecting) employees assigned to the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project whose employees are being managed.</param>
        /// <returns>A view with the selectable employee list, or a redirect if the project does not exist or the user is not authorized.</returns>
        [HttpGet]
        public async Task<IActionResult> ManageEmployeesInProject(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }


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
                IEnumerable<SelectEmployeesForProjectFormModel> employees =
                    await this.projectService.AllEmployeesForProjectAsync(id);
                return this.View(employees);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error loading employees for project {ProjectId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while adding employee to project. Please try again later or contact administrator!";
                return this.RedirectToAction("Mine", "Employee", new { id });
            }
        }

        /// <summary>
        /// Saves the selected employee assignments for the specified project.
        /// </summary>
        /// <param name="id">The unique identifier of the project to update.</param>
        /// <param name="selectedEmployee">A collection of employee IDs to assign to the project.</param>
        /// <returns>Redirects to the project details page on success, or a redirect if the user is not authorized.</returns>
        [HttpPost]
        public async Task<IActionResult> ManageEmployeesInProject(string id, IEnumerable<string> selectedEmployee)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }


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
                await this.projectService.SaveAllEmployeesForProjectAsync(id, selectedEmployee);
                this.TempData[SuccessMessage] = "Employee changed successfully.";
                return this.RedirectToAction("Details", "Project", new { id });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error saving employees for project {ProjectId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while adding employee to project. Please try again later or contact administrator!";
                return this.RedirectToAction("Mine", "Employee", new { id });
            }
        }



        /// <summary>
        /// Streams a file from the server's attachment directory to the client for download.
        /// </summary>
        /// <param name="filename">The name of the file to download (must reside within the attachment directory).</param>
        /// <returns>A <see cref="FileStreamResult"/> for the requested file, or <see cref="BadRequestResult"/> / <see cref="NotFoundResult"/> if the file is invalid or missing.</returns>
        [HttpGet]
        public IActionResult GetFileStreamResult(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return BadRequest();

            string baseDir = Path.GetFullPath(Path.Combine(this.hostingEnvironment.WebRootPath, "attachment"));
            string fullPath = Path.GetFullPath(Path.Combine(baseDir, filename));

            if (!fullPath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
                return BadRequest();

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            string contentType = projectService.GetContentType(filename);
            var stream = new MemoryStream(System.IO.File.ReadAllBytes(fullPath));
            return new FileStreamResult(stream, new MediaTypeHeaderValue(contentType))
            {
                FileDownloadName = filename
            };
        }

        /// <summary>
        /// Removes the image associated with the specified project from Cloudinary and clears the image metadata in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the project whose image should be deleted.</param>
        /// <returns>Redirects to the project details page on success, or to the home page on unexpected error.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteImage(string id)
        {
            bool projectExists = await this.projectService.ProjectExistsByIdAsync(id);
            if (!projectExists)
            {
                this.TempData[ErrorMessage] = "Project with provided id does not exist.";
                return this.RedirectToAction("Mine", "Employee");
            }
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

                if (currentProject.PublicId != null)
                {
                    await this.cloudinaryService.DeletePhotoAsync(currentProject.PublicId);
                    currentProject.UrlPicturePath = null;
                    currentProject.PublicId = null;
                    currentProject.ImageName = null;
                }

                await dbContext.SaveChangesAsync();

                return this.RedirectToAction("Details", "Project", new { id });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting image for project {ProjectId}.", id);
                this.TempData[ErrorMessage] =
                    "An error occurred while editing the project. Please try again later or contact administrator!";
                return GeneralError();

            }
        }
    }
}