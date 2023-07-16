namespace CivilEngineerCMS.Web.ViewModels.Project.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum ProjectSorting
    {
        [Display(Name = "Project Name")]
        ProjectName = 0,
        [Display(Name = "Project Status")]
        Status = 1,
        [Display(Name = "Project Description")]
        Description = 2,
        [Display(Name = "Client Name")]
        ClientName = 3,
        [Display(Name = "Client Email")]
        ClientEmail = 4,
        [Display(Name = "Client Phone")]
        ClientPhone = 5,
        [Display(Name = "Manager Name")]
        ManagerName = 6,
        [Display(Name = "Manager Email")]
        ManagerEmail = 7,
        [Display(Name = "Manager Phone")]
        ManagerPhone = 8,
        [Display(Name = "Not filtered")]
        NotFiltered = 9,

    }
}
