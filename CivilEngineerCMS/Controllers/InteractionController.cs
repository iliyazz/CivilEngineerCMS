namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class InteractionController : BaseController
    {
        public async Task<IActionResult> All()
        {
            return View();
        }
    }
}
