namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using CivilEngineerCMS.Common;
    using System.ComponentModel.DataAnnotations;
    using Client;
    using Manager;
    using static CivilEngineerCMS.Common.EntityValidationConstants.Project;

    public class AddProjectFormModel
    {
        public AddProjectFormModel()
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

        //[Required]
        //[Display(Name = "Client Fullname")]
        //public string ClientFullname { get; set; } = null!;

        [MaxLength(UrlMaxLength)]
        [Display(Name = "Image link")]
        public string? UrlPicturePath { get; set; }

        [Required]
        [Display(Name = "Project Status")]
        public ProjectStatusEnums Status { get; set; }

        [Required]
        [Display(Name = "Project End Date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd-MM-yyyy}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ProjectEndDate { get; set; }

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
