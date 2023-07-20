namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    using ViewModels.Interaction;

    using static Common.NotificationMessagesConstants;

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
            if (!await this.interactionService.InteractionExistsByProjectIdAsync(id))
            {
                this.TempData[InfoMessage] = "There are no interaction about this project. Create the first one.";
                return RedirectToAction("Add", "Interaction", new { id = id });
            }

            IEnumerable<AddAndEditInteractionFormModel> viewModel =
                await this.interactionService.AllInteractionsByProjectIdAsync(id);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add(string id)
        {
            AddAndEditInteractionFormModel formModel = new AddAndEditInteractionFormModel
            {
                ProjectId = Guid.Parse(id),
                Date = DateTime.UtcNow,
            };
            return View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string id, AddAndEditInteractionFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            try
            {
                await this.interactionService.CreateInteractionAsync(id, formModel);

                this.TempData[SuccessMessage] = $"Interaction added successfully.";
                return this.RedirectToAction("All", "Interaction", new { id = id });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while adding the interaction. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string projectId, string interactionId)
        {
            bool isInteractionExists = await this.interactionService.InteractionExistsByProjectIdAsync(projectId);
            if (!isInteractionExists)
            {
                this.TempData[ErrorMessage] = "Interaction does not exist.";
                return RedirectToAction("All", "Interaction");
            }

            AddAndEditInteractionFormModel viewModel =
                await this.interactionService.GetInteractionForEditByProjectIdAsync(projectId, interactionId);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string interactionId, string projectId,
            AddAndEditInteractionFormModel formModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(formModel);
            }

            try
            {
                await this.interactionService.EditInteractionByProjectIdAsync(interactionId, formModel);

                this.TempData[SuccessMessage] = $"Interaction edited successfully.";
                return this.RedirectToAction("All", "Interaction", new { id = projectId });
            }
            catch (Exception _)
            {
                this.ModelState.AddModelError(string.Empty,
                    "An error occurred while editing the interaction. Please try again later or contact administrator!");
                return this.View(formModel);
            }
        }
    }
}