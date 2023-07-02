namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class TaskController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
