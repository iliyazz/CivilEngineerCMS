namespace CivilEngineerCMS.Web.Infrastructure.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    /// <summary>
    /// DecimalModeBinderProvider is used to bind decimal values from different cultures.
    /// </summary>
    public class DecimalModeBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
            {
                return new DecimalModelBinders();
            }

            return null!;
        }
    }
}