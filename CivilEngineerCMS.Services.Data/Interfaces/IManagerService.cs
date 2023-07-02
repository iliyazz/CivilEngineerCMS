namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using Web.ViewModels.Manager;

    public interface IManagerService
    {
        Task<IEnumerable<ProjectSelectManagerFormModel>> AllManagersAsync();
    }
}
