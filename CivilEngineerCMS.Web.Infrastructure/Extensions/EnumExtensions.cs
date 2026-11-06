namespace CivilEngineerCMS.Web.Infrastructure.Extensions
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the [Display(Name = ...)] text of an enum value (the English localization key),
        /// or the value name when the attribute is missing.
        /// </summary>
        public static string GetDisplayName(this Enum value)
        {
            string name = value.ToString();
            return value.GetType().GetField(name)?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? name;
        }
    }
}
