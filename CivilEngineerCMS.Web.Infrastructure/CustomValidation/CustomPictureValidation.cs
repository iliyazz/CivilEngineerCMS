using System.ComponentModel.DataAnnotations;

namespace CivilEngineerCMS.Web.Infrastructure.CustomValidation
{
    public class CustomPictureValidation : ValidationAttribute
    {
        public ValidationResult IsValidFile(object value, ValidationContext validationContext)
        {
            var contentType = value as string;
            if (contentType == null)
            {
                return ValidationResult.Success;
            }

            if (contentType.Contains("image"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Only jpg, gif and png files are allowed.");
        }
    }
}
