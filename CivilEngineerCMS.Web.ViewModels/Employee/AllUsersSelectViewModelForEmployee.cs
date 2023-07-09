namespace CivilEngineerCMS.Web.ViewModels.Employee
{
    using System.ComponentModel.DataAnnotations;

    public class AllUsersSelectViewModelForEmployee
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;
    }
}
