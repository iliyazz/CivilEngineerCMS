namespace CivilEngineerCMS.Web.ViewModels.Manager
{
    public class SelectEmployeesAndManagerForProjectFormModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public string FullName => FirstName + " " + LastName;
        public bool IsChecked { get; set; }
    }
}
