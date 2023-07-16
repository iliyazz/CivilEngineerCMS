namespace CivilEngineerCMS.Web.ViewModels.Project;

public class MineViewModel
{
    public string ProjectName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ManagerName { get; set; } = null!;
    public DateTime ProjectCreatedDate { get; set; }
    public DateTime ProjectEndDate { get; set; }
    public string ClientName { get; set; } = null!;
    public int StatusId { get; set; } 
}

