//using System.Web.Http;

using CivilEngineerCMS.Data.Models;

using Microsoft.AspNetCore.Identity;

namespace CivilEngineerCMS.WebApi.Controllers;

using CivilEngineerCMS.Services.Data.Interfaces;
using CivilEngineerCMS.WebApi.Credentials;

using Microsoft.AspNetCore.Mvc;

[Route("api")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IEmployeeService employeeService;
    private readonly IClientService clientService;
    private readonly IProjectService projectService;

    public ProjectsController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IProjectService projectService,
        IEmployeeService employeeService,
        IClientService clientService)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.projectService = projectService;
        this.employeeService = employeeService;
        this.clientService = clientService;
    }


    [HttpPost]
    [Route("projects")]
    public async Task<IActionResult> Login([FromBody] UserCredentials credentials)
    {
        // In a real application, you'd verify the credentials against a database or external service.
        var result = await this.signInManager.PasswordSignInAsync(credentials.Username, credentials.Password, false, false);

        if (result.Succeeded)
        {
            var user = await this.userManager.FindByNameAsync(credentials.Username);
            var isAdmin = await this.userManager.IsInRoleAsync(user, "Administrator");
            var isEmployee = await this.employeeService.IsEmployeeAsync(user.Id.ToString());
            var isClient = await this.clientService.IsClientByUserIdAsync(user.Id.ToString());
            if (isAdmin)
            {
                var allProjects = await this.projectService.AllProjectsAsyncApi();
                return this.Ok(allProjects);
            }

            if (isEmployee)
            {
                var employeeId = await this.employeeService.GetEmployeeIdByUserIdAsync(user.Id.ToString());
                var myProjects = await this.employeeService.AllProjectsByEmployeeIdAsync(employeeId);
                return this.Ok(myProjects);
            }

            if (isClient)
            {
                var myProjects = await this.clientService.AllProjectsByUserIdAsync(user.Id.ToString());
                return this.Ok(myProjects);
            }


            return this.Ok("Authentication failed");

        }

        // Authentication failed
        return Unauthorized("Authentication failed");
    }
}