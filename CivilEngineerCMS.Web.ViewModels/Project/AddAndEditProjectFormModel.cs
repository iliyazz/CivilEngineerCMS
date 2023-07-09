namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using CivilEngineerCMS.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Client;
    using Manager;
    using static CivilEngineerCMS.Common.EntityValidationConstants.Project;

    public class AddAndEditProjectFormModel
    {
        public AddAndEditProjectFormModel()
        {
            this.Managers = new HashSet<ProjectSelectManagerFormModel>();
            this.Clients = new HashSet<ProjectSelectClientFormModel>();
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
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public string /*DateTime*/ ProjectEndDate { get; set; }

        [Required]
        [Display(Name = "Project Manager")]
        public Guid ManagerId { get; set; }
        public IEnumerable<ProjectSelectManagerFormModel> Managers { get; set; }

        [Required]
        [Display(Name = "Client name")]
        public Guid ClientId { get; set; }
        public IEnumerable<ProjectSelectClientFormModel> Clients { get; set; }

    }
}
