namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using CivilEngineerCMS.Common;

    public class DetailsProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? UrlPicturePath { get; set; }
        public ProjectStatusEnums Status { get; set; }
        public string ProjectStartDate { get; set; } = null!;
        public string ProjectEndDate { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
        public string ClientName { get; set; } = null!;
    }
}
