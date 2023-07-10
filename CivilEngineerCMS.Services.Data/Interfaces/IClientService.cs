namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Client;

    public interface IClientService
    {
        Task<IEnumerable<MineClientManagerProjectViewModel>> AllProjectsByUserIdAsync(string userId);
        Task<IEnumerable<ProjectSelectClientFormModel>> AllClientsAsync();
        Task<IEnumerable<AllClientViewModel>> AllClientsForViewAsync();
        Task CreateClientAsync(CreateClientFormModel formModel);
        Task<DetailsClientViewModel> DetailsClientAsync(string clientId);
        Task<bool> ClientExistsByIdAsync(string id);
        //Task<IEnumerable<AllClientSelectForEditViewModel>> GetAllClientsForEditAsync();
    }
}