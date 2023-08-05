namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    using System.ComponentModel.DataAnnotations;
    using static CivilEngineerCMS.Common.EntityValidationConstants.Employee;
    public class AllEmployeeViewModel
    {

        public Guid Id { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        [MinLength(FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        [MinLength(LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(JobTitleMaxLength)]
        [MinLength(JobTitleMinLength)]
        public string JobTitle { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
    }
}


