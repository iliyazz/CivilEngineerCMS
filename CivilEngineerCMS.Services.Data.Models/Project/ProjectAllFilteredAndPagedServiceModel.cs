namespace CivilEngineerCMS.Services.Data.Models.Project
{
    using Web.ViewModels.Project;

    public class ProjectAllFilteredAndPagedServiceModel
    {
        public ProjectAllFilteredAndPagedServiceModel()
        {
            this.Projects = new HashSet<ProjectAllViewModel>();
        }

        public int TotalProjectsCount { get; set; }
        public IEnumerable<ProjectAllViewModel> Projects { get; set; }
    }
}