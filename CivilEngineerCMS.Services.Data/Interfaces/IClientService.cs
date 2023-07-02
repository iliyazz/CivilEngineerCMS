namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using Web.ViewModels.Client;

    public interface IClientService
    {
        Task<IEnumerable<MineClientManagerProjectViewModel>> AllProjectsByUserIdAsync(string userId);
    }
}
