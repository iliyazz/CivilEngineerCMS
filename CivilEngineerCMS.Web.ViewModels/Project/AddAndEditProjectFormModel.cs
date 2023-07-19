namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using System.ComponentModel.DataAnnotations;

    using CivilEngineerCMS.Common;

    using Client;
    using Employee;
    using Manager;

    using static CivilEngineerCMS.Common.EntityValidationConstants.Project;

    public class AddAndEditProjectFormModel
    {
        public AddAndEditProjectFormModel()
        {
            this.Managers = new HashSet<SelectEmployeesAndManagerForProjectFormModel>();
            this.Clients = new HashSet<SelectClientForProjectFormModel>();
            this.Employees = new HashSet<AllEmployeeViewModel>();
        }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Project Title")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        [Display(Name = "Project Description")]
        public string Description { get; set; } = null!;

        [MaxLength(UrlMaxLength)]
        [Display(Name = "Image link")]
        public string? UrlPicturePath { get; set; }

        [Required]
        [Display(Name = "Project Status")]
        public ProjectStatusEnums Status { get; set; }

        [Required]
        [Display(Name = "Project End Date")]
        public string ProjectEndDate { get; set; } = null!;

        [Required]
        [Display(Name = "Project Manager")]
        public Guid ManagerId { get; set; }
        public IEnumerable<SelectEmployeesAndManagerForProjectFormModel> Managers { get; set; }

        [Required]
        [Display(Name = "Client name")]
        public Guid ClientId { get; set; }
        public IEnumerable<SelectClientForProjectFormModel> Clients { get; set; }


        public Guid EmployeeId { get; set; }
        public IEnumerable<AllEmployeeViewModel> Employees { get; set; }
    }
}
