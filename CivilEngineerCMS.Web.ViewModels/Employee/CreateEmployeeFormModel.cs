namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    using System.ComponentModel.DataAnnotations;
    using static CivilEngineerCMS.Common.EntityValidationConstants.Employee;

    public class CreateEmployeeFormModel
    {
        public CreateEmployeeFormModel()
        {
            this.Users = new HashSet<AllUsersSelectViewModelForEmployee>();
        }
        [Required]
        [Display(Name = "Email")]
        public Guid UserId { get; set; }

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
        [Phone]
        [MaxLength(PhoneNumberMaxLength)]
        [MinLength(PhoneNumberMinLength)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        [MinLength(AddressMinLength)]
        [Display(Name = "Address")]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(JobTitleMaxLength)]
        [MinLength(JobTitleMinLength)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = null!;

        public IEnumerable<AllUsersSelectViewModelForEmployee> Users { get; set; }
    }
}
