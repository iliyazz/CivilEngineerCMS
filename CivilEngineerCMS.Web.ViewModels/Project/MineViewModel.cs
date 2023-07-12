namespace CivilEngineerCMS.Web.ViewModels.Project;

public class MineViewModel
{
    public string ProjectName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ManagerName { get; set; } = null!;
    public DateTime ProjectCreatedDate { get; set; }
    public DateTime ProjectEndDate { get; set; }
    public string ClientName { get; set; } = null!;
    public string Status { get; set; } = null!;
}
/*
    public class MineClientManagerProjectViewModel
    {
        public DateTime ProjectCreatedDate { get; set; }
        public string ProjectName { get; set; } = null!;
        public DateTime ProjectEndDate { get; set; }
        public string ManagerName { get; set; } = null!;
        public string ManagerPhoneNumber { get; set; } = null!;

public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid ManagerId { get; set; }
    public string? UrlPicturePath { get; set; }
    public ProjectStatusEnums Status { get; set; }
    public DateTime ProjectCreatedDate { get; set; }
    public DateTime ProjectEndDate { get; set; }
    }
 */
