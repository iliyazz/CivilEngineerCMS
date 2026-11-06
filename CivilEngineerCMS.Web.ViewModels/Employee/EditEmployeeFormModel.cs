namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    using System.ComponentModel.DataAnnotations;
    using static CivilEngineerCMS.Common.EntityValidationConstants.Employee;

    public class EditEmployeeFormModel
    {
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; } = null!;
        [Required]
        [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = null!;
    }
}
