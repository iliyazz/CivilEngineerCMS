using CivilEngineerCMS.Common;

namespace CivilEngineerCMS.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using System.Data;

    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Services.Data.Models.Statistics;
    using static GeneralApplicationConstants;


    [Route("api/statistics")]
    [ApiController]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IProjectService projectService;

        public StatisticsApiController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StatisticsServiceModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatistics()
        {

            try
            {
                StatisticsServiceModel serviceModel = await this.projectService.GetStatisticsAsync();
                return this.Ok(serviceModel);
            }
            catch (Exception)
            {
               return this.BadRequest();
            }


        }
    }
}
