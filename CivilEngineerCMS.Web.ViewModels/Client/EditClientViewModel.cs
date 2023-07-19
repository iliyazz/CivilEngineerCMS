//namespace CivilEngineerCMS.Web.ViewModels.Client
//{
//    using System.ComponentModel.DataAnnotations;
//    using static Common.EntityValidationConstants.Client;

//    public class EditClientViewModel
//    {
//        public EditClientViewModel()
//        {
//            this.Clients = new HashSet<SelectClientForProjectFormModel>();
//        }

//        [Required]
//        public Guid ClientId { get; set; }
//        public IEnumerable<SelectClientForProjectFormModel> Clients { get; set; }
//        [Required]
//        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
//        [Display(Name = "First Name")]
//        public string FirstName { get; set; } = null!;

//        [Required]
//        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
//        [Display(Name = "Last Name")]
//        public string LastName { get; set; } = null!;

//        [Required]
//        [Phone]
//        [Display(Name = "Phone Number")]
//        public string PhoneNumber { get; set; } = null!;

//        [Required]
//        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
//        public string Address { get; set; } = null!;

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; } = null!;
//    }
//}
