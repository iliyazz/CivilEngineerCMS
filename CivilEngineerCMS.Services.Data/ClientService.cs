using CivilEngineerCMS.Services.Data.Interfaces;

namespace CivilEngineerCMS.Services.Data
{
    using System.Security.Claims;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;
    using CivilEngineerCMS.Web.ViewModels.Client;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Client;
    using Web.ViewModels.Employee;
    using Task = System.Threading.Tasks.Task;

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

        public async Task<IEnumerable<AllClientViewModel>> AllClientsForViewAsync()
        {
            IEnumerable<AllClientViewModel> allClients = await dbContext
                .Clients
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Select(c => new AllClientViewModel()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber
                })
                .ToListAsync();
            return allClients;
        }


        public async Task CreateClientAsync(CreateClientFormModel formModel)
        {
  

            Client client = new Client
            {
                UserId = formModel.UserId,
                FirstName = formModel.FirstName,
                LastName = formModel.LastName,
                PhoneNumber = formModel.PhoneNumber,
                Address = formModel.Address,
            };
            await this.dbContext.Clients.AddAsync(client);
            await this.dbContext.SaveChangesAsync();
        }


    }
}
