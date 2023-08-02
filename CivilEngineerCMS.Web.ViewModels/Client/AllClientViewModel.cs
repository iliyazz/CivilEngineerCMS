namespace CivilEngineerCMS.Web.ViewModels.Client
{
    using System.ComponentModel.DataAnnotations;

    public class AllClientViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
