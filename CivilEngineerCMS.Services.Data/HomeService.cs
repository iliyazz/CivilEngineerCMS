namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;

    using Interfaces;

    using Microsoft.EntityFrameworkCore;

    using Web.ViewModels.Home;

    public class HomeService : IHomeService
    {
        private readonly CivilEngineerCmsDbContext dbContext;

        public HomeService(CivilEngineerCmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        /// <summary>
        /// This method return all projects for index page
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IndexViewModel>> AllIndexProjectsAsync()
        {
            IEnumerable<IndexViewModel> allProjects = await this.dbContext
                .Projects
                //.Where(p => !string.IsNullOrWhiteSpace(p.UrlPicturePath))
                .Where(p => p.ImageContent != null)
                .OrderBy(x => Guid.NewGuid())
                .Select(p => new IndexViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    //UrlPicturePath = p.UrlPicturePath,
                    ImageContent = p.ImageContent,
                    ImageName = p.ImageName,
                })
                .Take(5)
                .ToListAsync();
            return allProjects;
        }
    }
}