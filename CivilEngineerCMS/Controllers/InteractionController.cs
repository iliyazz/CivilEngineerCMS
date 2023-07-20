namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using ViewModels.Interaction;

    public class InteractionController : BaseController
    {
        private readonly IInteractionService interactionService;

        public InteractionController(IInteractionService interactionService)
        {
            this.interactionService = interactionService;
        }




        [HttpGet]
        public async Task<IActionResult> All(string id)
        {
            IEnumerable<AddAndEditInteractionFormModel> viewModel = await this.interactionService.AllInteractionsByProjectIdAsync(id);
            return View(viewModel);
        }
    }
}
/*
        public async Task<IActionResult> All()
        {
            IEnumerable<AllEmployeeViewModel> viewModel = await this.employeeService.AllEmployeesAsync();
            return this.View(viewModel);
        }
 */
