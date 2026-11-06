namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;


    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class BaseController : Controller
    {
        private IStringLocalizer<SharedResource>? localizer;

        /// <summary>
        /// Localizer for user-facing messages (notifications, model errors). Keys are the English texts.
        /// </summary>
        protected IStringLocalizer<SharedResource> L =>
            this.localizer ??= this.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
    }
}
