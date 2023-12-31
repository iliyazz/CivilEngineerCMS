﻿namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using CivilEngineerCMS.Web.ViewModels.Client;

    public interface IClientService
    {
        Task<IEnumerable<MineClientManagerProjectViewModel>> AllProjectsByUserIdAsync(string userId);
        Task<IEnumerable<SelectClientForProjectFormModel>> AllClientsAsync();
        Task<IEnumerable<AllClientViewModel>> AllClientsForViewAsync();
        Task CreateClientAsync(CreateClientFormModel formModel);
        Task<DetailsClientViewModel> DetailsClientAsync(string clientId);
        Task<bool> ClientExistsByIdAsync(string id);
        Task<EditClientFormModel> GetClientForEditByIdAsync(string clientId);
        Task EditClientByIdAsync(string clientId, EditClientFormModel formModel);
        Task<ClientPreDeleteViewModel> GetClientForPreDeleteByIdAsync(string clientId);
        Task DeleteClientByIdAsync(string clientId);
        Task<string> GetClientIdByProjectIdAsync(string projectId);
        Task<bool> IsClientByUserIdAsync(string userId);
        Task<string> GetClientIdByUserIdAsync(string userId);
    }
}