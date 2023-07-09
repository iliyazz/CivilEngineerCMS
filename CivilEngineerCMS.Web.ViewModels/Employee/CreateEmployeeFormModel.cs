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
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        [MinLength(FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        [MinLength(LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone]
        [MaxLength(PhoneNumberMaxLength)]
        [MinLength(PhoneNumberMinLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        [MinLength(AddressMinLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(JobTitleMaxLength)]
        [MinLength(JobTitleMinLength)]
        public string JobTitle { get; set; } = null!;

        public IEnumerable<AllUsersSelectViewModelForEmployee> Users { get; set; }
    }
}
