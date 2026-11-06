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
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        [MinLength(LastNameMinLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(JobTitleMaxLength)]
        [MinLength(JobTitleMinLength)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = null!;

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
    }
}


