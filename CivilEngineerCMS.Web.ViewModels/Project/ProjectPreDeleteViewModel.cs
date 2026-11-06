using CivilEngineerCMS.Common;

namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using System.ComponentModel.DataAnnotations;

    public class ProjectPreDeleteViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Project Title")]
        public string Name { get; set; } = null!;

        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Display(Name = "Image link")]
        public string? UrlPicturePath { get; set; }

        [Display(Name = "Status")]
        public ProjectStatusEnums Status { get; set; }

        [Display(Name = "Start Date")]
        public string ProjectStartDate { get; set; } = null!;

        [Display(Name = "End Date")]
        public string ProjectEndDate { get; set; } = null!;

        [Display(Name = "Manager")]
        public string ManagerName { get; set; } = null!;

        [Display(Name = "Client")]
        public string ClientName { get; set; } = null!;
    }
}

