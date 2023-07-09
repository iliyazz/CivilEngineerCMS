namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Client;
    using Web.ViewModels.Client;
    using Web.ViewModels.Employee;

    public interface IClientService
    {
        Task<IEnumerable<MineClientManagerProjectViewModel>> AllProjectsByUserIdAsync(string userId);
        Task<IEnumerable<ProjectSelectClientFormModel>> AllClientsAsync();
        Task<IEnumerable<AllClientViewModel>> AllClientsForViewAsync();
        Task CreateClientAsync(CreateAndEditClientFormModel formModel);
        //Task<CreateAndEditClientFormModel> GetClientForEditByIdAsync(string clientId);
        //Task<CreateAndEditClientFormModel> EditClientByIdAsync(string clientId);
    }
}