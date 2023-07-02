namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BoardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
