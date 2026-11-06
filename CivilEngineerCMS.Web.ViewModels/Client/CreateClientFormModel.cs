namespace CivilEngineerCMS.Web.ViewModels.Client
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.Client;

    public class CreateClientFormModel
    {
        public CreateClientFormModel()
        {
            this.Users = new HashSet<AllUsersSelectViewModelForClient>();
        }

        [Required]
        [Display(Name = "Email")]
        public Guid UserId { get; set; }

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
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        [Display(Name = "Address")]
        public string Address { get; set; } = null!;

        public IEnumerable<AllUsersSelectViewModelForClient> Users { get; set; }
    }
}