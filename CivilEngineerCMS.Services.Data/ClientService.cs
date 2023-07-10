using CivilEngineerCMS.Services.Data.Interfaces;

namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;
    using CivilEngineerCMS.Web.ViewModels.Client;

    using Microsoft.EntityFrameworkCore;

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
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.User.Email,
                    Address = c.Address
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
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.User.Email,
                    Address = c.Address
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

        public async Task<DetailsClientViewModel> DetailsClientAsync(string clientId)
        {
            DetailsClientViewModel client = await this.dbContext
                .Clients
                .Include(c => c.User)
                .Where(c => c.Id.ToString() == clientId)
                .Select(c => new DetailsClientViewModel()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.User.Email,
                    Address = c.Address
                })
                .FirstAsync();

            var result = new DetailsClientViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                Address = client.Address
            };
            return result;
        }

        public async Task<bool> ClientExistsByIdAsync(string id)
        {
            return await this.dbContext.Clients.AnyAsync(c => c.Id.ToString() == id);
        }

        public async Task<EditClientFormModel> GetClientForEditByIdAsync(string clientId)
        {
            Client client = await this.dbContext
                .Clients
                .Include(c => c.User)
                .Where(c => c.Id.ToString() == clientId)
                .FirstAsync();
            string email = client.User.Email;
            var result = new EditClientFormModel
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Address = client.Address,
                Email = email
            };
            return result;
        }

        public async Task EditClientByIdAsync(string clientId, EditClientFormModel formModel)
        {
            Client client = await this.dbContext                .Clients
                .Include(c => c.User)
                .Where(c => c.Id.ToString() == clientId)
                .FirstAsync();
            client.FirstName = formModel.FirstName;
            client.LastName = formModel.LastName;
            client.PhoneNumber = formModel.PhoneNumber;
            client.Address = formModel.Address;
            client.User.Email = formModel.Email;
            await this.dbContext.SaveChangesAsync();
        }
    }
}

