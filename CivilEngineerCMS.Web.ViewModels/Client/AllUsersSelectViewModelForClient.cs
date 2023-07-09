namespace CivilEngineerCMS.Web.ViewModels.Client
{
    using System.ComponentModel.DataAnnotations;

    public class AllUsersSelectViewModelForClient
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;
    }
}
