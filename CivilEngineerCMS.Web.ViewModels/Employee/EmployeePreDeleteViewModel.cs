namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    using System.ComponentModel.DataAnnotations;

    public class EmployeePreDeleteViewModel
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        [Display(Name = "Job title")]
        public string JobTitle { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
