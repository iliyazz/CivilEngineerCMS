namespace CivilEngineerCMS.Services.Data.Interfaces;

using Web.ViewModels.Home;

public interface IHomeService
{
    Task<IEnumerable<IndexViewModel>> AllIndexProjectsAsync();
}