namespace CivilEngineerCMS.Web.ViewModels.Client
{
    public class MineClientManagerProjectViewModel
    {
        public DateTime ProjectCreatedDate { get; set; }
        public string ProjectName { get; set; } = null!;
        public DateTime ProjectEndDate { get; set; }
        public string ManagerName { get; set; } = null!;
        public string ManagerPhoneNumber { get; set; } = null!;
    }
}
