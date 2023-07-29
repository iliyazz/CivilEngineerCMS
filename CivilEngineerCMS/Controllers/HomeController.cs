namespace CivilEngineerCMS.Web.Controllers;

using Data.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Services.Data.Interfaces;

using ViewModels.Home;

public class HomeController : BaseController
{
    private readonly IHomeService homeService;

    public HomeController(IHomeService homeService, UserManager<ApplicationUser> userManager)
    {
        this.homeService = homeService;
    }


    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        IEnumerable<IndexViewModel> viewModel = await this.homeService.AllIndexProjectsAsync();

        return View(viewModel);
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        if (statusCode == 400 || statusCode == 404)
        {
            return this.View("Error404");
        }

        if (statusCode == 401)
        {
            return this.View("Error401");
        }

        return this.View();
    }
}