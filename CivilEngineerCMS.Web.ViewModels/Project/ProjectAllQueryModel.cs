namespace CivilEngineerCMS.Web.ViewModels.Project
{
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Enums;

    using static CivilEngineerCMS.Common.GeneralApplicationConstants;

    public class ProjectAllQueryModel
    {
        public ProjectAllQueryModel()
        {
            this.CurrentPage = DefaultPageNumber;
            this.ProjectsPerPage = EntitiesPerPage;
            this.Statuses = new HashSet<string>();
            this.Projects = new HashSet<ProjectAllViewModel>();
        }
        public ProjectStatusEnums? Status { get; set; }
        [Display(Name = "Search by word")]
        public string? SearchString { get; set; }
        [Display(Name = "Sort project by")]
        public ProjectSorting ProjectSorting { get; set; }
        public int CurrentPage { get; set; }
        [Display(Name = "Projects per page")]
        public int ProjectsPerPage { get; set; }
        public int TotalProjects { get; set; }
        public IEnumerable<string> Statuses { get; set; }
        public IEnumerable<ProjectAllViewModel> Projects { get; set; }
    }
}
