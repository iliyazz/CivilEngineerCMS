namespace CivilEngineerCMS.Services.Data.Interfaces;

using System.Security.Claims;

using Web.ViewModels.Home;

public interface IHomeService
{
    Task<IEnumerable<IndexViewModel>> AllIndexProjectsAsync();
}