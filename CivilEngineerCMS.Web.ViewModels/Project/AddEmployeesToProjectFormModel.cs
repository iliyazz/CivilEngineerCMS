namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using Employee;

    public class AddEmployeesToProjectFormModel
    {
        public AddEmployeesToProjectFormModel()
        {
            this.Employees = new List<EmployeeForProjectFormModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<EmployeeForProjectFormModel> Employees { get; set; }
    }
}
