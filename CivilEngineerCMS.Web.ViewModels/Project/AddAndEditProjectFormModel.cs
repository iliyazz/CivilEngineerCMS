namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using System.ComponentModel.DataAnnotations;

    using CivilEngineerCMS.Common;

    using Client;

    using Employee;

    using Manager;

    using Microsoft.AspNetCore.Http;

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
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [MaxLength(UrlMaxLength)]
        [Display(Name = "Image link")]
        public string? UrlPicturePath { get; set; }

        [MaxLength(UrlMaxLength)]
        public string? PublicId { get; set; }

        [MaxLength(ImageNameMaxLength)]
        public string? ImageName { get; set; }


        public string? ContentType { get; set; }

        [Display(Name = "Project Image")]
        [DataType(DataType.Upload)]

        public IFormFile? ImageContent { get; set; }

        [Required]
        [Display(Name = "Status")]
        public ProjectStatusEnums Status { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public string ProjectEndDate { get; set; } = null!;

        [Required]
        [Display(Name = "Manager")]
        public Guid ManagerId { get; set; }
        public IEnumerable<SelectEmployeesAndManagerForProjectFormModel> Managers { get; set; }

        [Required]
        [Display(Name = "Client")]
        public Guid ClientId { get; set; }
        public IEnumerable<SelectClientForProjectFormModel> Clients { get; set; }


        public Guid EmployeeId { get; set; }
        public IEnumerable<AllEmployeeViewModel> Employees { get; set; }
    }
}
