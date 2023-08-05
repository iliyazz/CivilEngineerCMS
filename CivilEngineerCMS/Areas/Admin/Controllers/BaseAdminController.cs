namespace CivilEngineerCMS.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class BaseAdminController : Controller
    {

    }
}