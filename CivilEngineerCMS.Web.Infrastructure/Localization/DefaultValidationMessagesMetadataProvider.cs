namespace CivilEngineerCMS.Web.Infrastructure.Localization
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

    /// <summary>
    /// Data annotation messages are only localized when the attribute has an explicit ErrorMessage.
    /// This provider fills in the standard English templates for attributes that do not set one,
    /// so they can be looked up (and translated) through the shared resource file.
    /// </summary>
    public class DefaultValidationMessagesMetadataProvider : IValidationMetadataProvider
    {
        public const string RequiredMessage = "The {0} field is required.";
        public const string StringLengthMessage = "The field {0} must be a string with a maximum length of {1}.";
        public const string StringLengthWithMinimumMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.";
        public const string MaxLengthMessage = "The field {0} must be a string or array type with a maximum length of '{1}'.";
        public const string MinLengthMessage = "The field {0} must be a string or array type with a minimum length of '{1}'.";
        public const string RangeMessage = "The field {0} must be between {1} and {2}.";
        public const string PhoneMessage = "The {0} field is not a valid phone number.";
        public const string EmailAddressMessage = "The {0} field is not a valid e-mail address.";

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            IList<object> validators = context.ValidationMetadata.ValidatorMetadata;

            // Non-nullable value types are implicitly required by MVC, but that implicit attribute
            // never gets a localizable message. Adding it explicitly lets it be translated.
            Type modelType = context.Key.ModelType;
            if (context.Key.MetadataKind == ModelMetadataKind.Property &&
                modelType.IsValueType &&
                Nullable.GetUnderlyingType(modelType) == null &&
                !validators.OfType<RequiredAttribute>().Any())
            {
                validators.Add(new RequiredAttribute());
            }

            foreach (ValidationAttribute attribute in validators.OfType<ValidationAttribute>())
            {
                if (!string.IsNullOrEmpty(attribute.ErrorMessage) ||
                    !string.IsNullOrEmpty(attribute.ErrorMessageResourceName))
                {
                    continue;
                }

                string? message = attribute switch
                {
                    RequiredAttribute => RequiredMessage,
                    StringLengthAttribute sl => sl.MinimumLength > 0
                        ? StringLengthWithMinimumMessage
                        : StringLengthMessage,
                    MaxLengthAttribute => MaxLengthMessage,
                    MinLengthAttribute => MinLengthMessage,
                    RangeAttribute => RangeMessage,
                    PhoneAttribute => PhoneMessage,
                    EmailAddressAttribute => EmailAddressMessage,
                    _ => null
                };

                if (message != null)
                {
                    attribute.ErrorMessage = message;
                }
            }
        }
    }
}
