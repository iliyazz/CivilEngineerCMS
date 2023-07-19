namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using System.ComponentModel.DataAnnotations;

    public class ProjectAllViewModel
    {
        public string Id { get; set; } = null!;
        [Display(Name = "Project name")]
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Display(Name = "Manager name")]
        public string ManagerName { get; set; } = null!;
        [Display(Name = "Project created date")]
        public DateTime ProjectCreatedDate { get; set; }
        [Display(Name = "Project end date")]
        public DateTime ProjectEndDate { get; set; }
        [Display(Name = "Client name")]
        public string ClientName { get; set; } = null!;
        public string Status { get; set; } = null!;
        [Display(Name = "Client phone number")]
        public string ClientPhoneNumber { get; set; } = null!;
        [Display(Name = "Client email")]
        public string ClientEmail { get; set; } = null!;
        [Display(Name = "Manager phone number")]
        public string ManagerPhoneNumber { get; set; } = null!;
        [Display(Name = "Manager email")]
        public string ManagerEmail { get; set; } = null!;
    }
}
