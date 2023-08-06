namespace CivilEngineerCMS.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// This method is used to get the Id of the current user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        /// <summary>
        /// This method is used to check if the current user is an administrator.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdministratorRoleName);
        }
    }
}