namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Expenses;

    using Web.ViewModels.Interaction;
    public interface IInteractionService
    {
        Task<IEnumerable<AddAndEditInteractionFormModel>> AllInteractionsByProjectIdAsync(string projectId);
        Task<bool> InteractionExistsByProjectIdAsync(string projectId);
        Task CreateInteractionAsync(string id, AddAndEditInteractionFormModel formModel);
        Task<AddAndEditInteractionFormModel> GetInteractionForEditByProjectIdAsync(string projectId, string interactionId);
        Task EditInteractionForEditByProjectIdAsync(string projectId, AddAndEditInteractionFormModel formModel);
    }
}
