namespace CivilEngineerCMS.Web.ViewModels.Administrator
{
    public class SelectEmployeesForAdministratorFormModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsChecked { get; set; }
        public Guid UserId { get; set; }
    }
}