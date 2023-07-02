namespace CivilEngineerCMS.Web.ViewModels.Project;

public class AllProjectViewModel

{
    public DateTime ProjectCreatedDate { get; set; }
    public string Name { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string ManagerName { get; set; } = null!;
}