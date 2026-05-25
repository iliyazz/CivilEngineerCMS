namespace CivilEngineerCMS.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Data.Interfaces;
using Services.Data.Models.Statistics;


[ApiController]
[Authorize]
public class StatisticsApiController : ControllerBase
{
    private readonly IProjectService projectService;
    private readonly ILogger<StatisticsApiController> logger;

    public StatisticsApiController(IProjectService projectService, ILogger<StatisticsApiController> logger)
    {
        this.projectService = projectService;
        this.logger = logger;
    }



    [Route("api/statistics")]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(StatisticsServiceModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStatistics()
    {
        try
        {
            StatisticsServiceModel serviceModel = await this.projectService.GetStatisticsAsync();
            return this.Ok(serviceModel);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error retrieving statistics.");
            return this.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
