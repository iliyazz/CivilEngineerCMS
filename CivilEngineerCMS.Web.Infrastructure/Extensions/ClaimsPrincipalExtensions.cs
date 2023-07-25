namespace CivilEngineerCMS.Web.Infrastructure.Extensions
{
    using System.Security.Claims;
    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdministratorRoleName);
        }
    }
}
