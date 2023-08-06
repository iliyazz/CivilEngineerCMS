namespace CivilEngineerCMS.Web.Infrastructure.ModelBinders
{
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    /// <summary>
    /// DecimalModelBinders is used to bind decimal values from different cultures.
    /// </summary>
    public class DecimalModelBinders : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            if (bindingContext.ModelType == typeof(decimal))
            {
                string modelName = bindingContext.ModelName;
                ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

                if (valueProviderResult != ValueProviderResult.None && !string.IsNullOrWhiteSpace(valueProviderResult.FirstValue))
                {
                    decimal parsedValue = 0m;
                    bool binderSucceeded = false;

                    try
                    {
                        string formDecimalValue = valueProviderResult.FirstValue;
                        formDecimalValue = formDecimalValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        formDecimalValue = formDecimalValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        parsedValue = Convert.ToDecimal(formDecimalValue);
                        binderSucceeded = true;
                    }
                    catch (FormatException fe)
                    {
                        bindingContext.ModelState.AddModelError(bindingContext.ModelName, fe, bindingContext.ModelMetadata);
                    }

                    if (binderSucceeded)
                    {
                        bindingContext.Result = ModelBindingResult.Success(parsedValue);
                    }
                }

            }
            return Task.CompletedTask;
        }
    }
}
