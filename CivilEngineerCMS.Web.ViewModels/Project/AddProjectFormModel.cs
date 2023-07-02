namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using CivilEngineerCMS.Common;
    using System.ComponentModel.DataAnnotations;
    using Manager;
    using static CivilEngineerCMS.Common.EntityValidationConstants.Project;

    public class AddProjectFormModel
    {
        public AddProjectFormModel()
        {
            this.Managers = new HashSet<ProjectSelectManagerFormModel>();
        }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Project Title")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        [Display(Name = "Project Description")]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Client Fullname")]
        public string ClientFullname { get; set; } = null!;


        [MaxLength(UrlMaxLength)]
        [Display(Name = "Image link")]
        public string? UrlPicturePath { get; set; }

        [Required]
        [Display(Name = "Project Status")]
        public ProjectStatusEnums Status { get; set; }

        [Required]
        [Display(Name = "Project End Date")]
        public DateTime ProjectEndDate { get; set; }

        [Required]
        [Display(Name = "Project Manager")]
        public Guid ManagerId { get; set; }
        public IEnumerable<ProjectSelectManagerFormModel> Managers { get; set; }
    }
}
