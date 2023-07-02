namespace CivilEngineerCMS.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// This is custom user class that works with the default ASP.NET Core Identity.
    /// Can be used to add additional properties to the default IdentityUser class.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            //this.Id = Guid.NewGuid();
            this.Projects = new HashSet<Project>();
        } 

        public virtual ICollection<Project> Projects { get; set; }// = new HashSet<Project>();
    }
}
