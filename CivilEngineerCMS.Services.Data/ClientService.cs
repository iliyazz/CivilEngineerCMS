using CivilEngineerCMS.Services.Data.Interfaces;

namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Client;

    public class ClientService : IClientService
    {
        private readonly CivilEngineerCmsDbContext dbContext;

        public ClientService(CivilEngineerCmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<MineClientManagerProjectViewModel>> AllProjectsByUserIdAsync(string userId)
        {
            IEnumerable<MineClientManagerProjectViewModel> allProjectsByUserIdAsync = await dbContext
                .Projects
                .Where(p => p.Client.UserId.ToString() == userId)
                .OrderBy(pn => pn.Name)
                .Select(p => new MineClientManagerProjectViewModel
                {
                    ProjectCreatedDate = p.ProjectCreatedDate,
                    ProjectName = p.Name,
                    ProjectEndDate = p.ProjectEndDate,
                    ManagerName = p.Manager.FirstName + " " + p.Manager.LastName,
                    ManagerPhoneNumber = p.Manager.PhoneNumber
                })
                .ToListAsync();
            return allProjectsByUserIdAsync;
        }

        public async Task<IEnumerable<ProjectSelectClientFormModel>> AllClientsAsync()
        {
            IEnumerable<ProjectSelectClientFormModel> allClients = await dbContext
                .Clients
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Select(c => new ProjectSelectClientFormModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName
                })
                .ToListAsync();
            return allClients;
        }
    }
}


