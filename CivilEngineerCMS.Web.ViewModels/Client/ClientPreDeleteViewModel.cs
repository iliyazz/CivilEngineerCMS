namespace CivilEngineerCMS.Web.ViewModels.Client
{
    using System.ComponentModel.DataAnnotations;

    public class ClientPreDeleteViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
        [Display(Name = "Address")]
        public string Address { get; set; } = null!;
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
    }
}
 