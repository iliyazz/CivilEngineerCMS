namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    public class AddEmployeeToProjectFormModel
    {
        public AddEmployeeToProjectFormModel()
        {
            this.Employees = new HashSet<SelectEmployeesForProjectFormModel>();
        }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public Guid EmployeeId { get; set; }
        public IEnumerable<SelectEmployeesForProjectFormModel> Employees { get; set; }

    }
}
