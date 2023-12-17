namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using CivilEngineerCMS.Common;

    using Employee;

    public class DetailsProjectViewModel
    {
        public DetailsProjectViewModel()
        {
            this.Employees = new HashSet<DetailsEmployeeViewModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        //public string? UrlPicturePath { get; set; }
        public ProjectStatusEnums Status { get; set; }
        public string ProjectStartDate { get; set; } = null!;
        public string ProjectEndDate { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string ClientPhone { get; set; } = null!;
        public string ClientEmail { get; set; } = null!;
        public byte[]? ImageContent { get; set; }
        public string ImageName { get; set; } = null!;
        public string? UrlPicturePath { get; set; }
        public IEnumerable<DetailsEmployeeViewModel> Employees { get; set; }
    }
}
