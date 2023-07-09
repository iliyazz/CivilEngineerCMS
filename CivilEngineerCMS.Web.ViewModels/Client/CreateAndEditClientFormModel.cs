namespace CivilEngineerCMS.Web.ViewModels.Client
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using static Common.EntityValidationConstants.Client;

    public class CreateAndEditClientFormModel
    {

        public CreateAndEditClientFormModel()
        {
            this.Users = new HashSet<AllUsersSelectViewModelForClient>();

        }

        [Required]
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
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; } = null!;

        public  IEnumerable<AllUsersSelectViewModelForClient> Users { get; set; }
    }
}

