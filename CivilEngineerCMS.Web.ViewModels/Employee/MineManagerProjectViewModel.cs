namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    public class MineManagerProjectViewModel
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string ClientPhoneNumber { get; set; } = null!;
        public DateTime ProjectCreatedDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
    }
}
