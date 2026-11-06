namespace CivilEngineerCMS.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;

    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    [AllowAnonymous]
    public class LanguageController : Controller
    {
        /// <summary>
        /// Stores the chosen UI language in the culture cookie and returns to the page the user came from.
        /// </summary>
        /// <param name="culture">The culture name, e.g. "bg" or "en". Unsupported values fall back to the default culture.</param>
        /// <param name="returnUrl">The local URL to return to.</param>
        [HttpPost]
        public IActionResult Set(string culture, string? returnUrl)
        {
            if (!SupportedCultures.Contains(culture))
            {
                culture = DefaultCulture;
            }

            this.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = this.Request.IsHttps
                });

            if (string.IsNullOrEmpty(returnUrl) || !this.Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return this.LocalRedirect(returnUrl);
        }
    }
}
