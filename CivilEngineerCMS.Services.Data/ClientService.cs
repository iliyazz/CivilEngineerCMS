using CivilEngineerCMS.Services.Data.Interfaces;

namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;
    using CivilEngineerCMS.Web.ViewModels.Client;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Task = System.Threading.Tasks.Task;

    public class ClientService : IClientService
    {
        private readonly CivilEngineerCmsDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;


        public ClientService(CivilEngineerCmsDbContext dbContext, UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.userService = userService;
        }
        /// <summary>
        /// This method return all projects by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MineClientManagerProjectViewModel>> AllProjectsByUserIdAsync(string userId)
        {
            IEnumerable<MineClientManagerProjectViewModel> allProjectsByUserIdAsync = await dbContext
                .Projects
                .Where(p => p.Client.UserId.ToString() == userId && p.IsActive)
                .OrderBy(pn => pn.Name)
                .Select(p => new MineClientManagerProjectViewModel
                {
                    ProjectId = p.Id.ToString(),
                    ProjectCreatedDate = p.ProjectCreatedDate,
                    ProjectName = p.Name,
                    ProjectEndDate = p.ProjectEndDate,
                    ManagerName = p.Manager.FirstName + " " + p.Manager.LastName,
                    ManagerPhoneNumber = p.Manager.PhoneNumber
                })
                .ToListAsync();
            return allProjectsByUserIdAsync;
        }

        /// <summary>
        /// This method return all clients
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectClientForProjectFormModel>> AllClientsAsync()
        {
            IEnumerable<SelectClientForProjectFormModel> allClients = await dbContext
                .Clients
                .Include(c => c.User)
                .Where(c => c.IsActive || c.User.LockoutEnd != null)
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Select(c => new SelectClientForProjectFormModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.User.Email,
                    Address = c.Address,
                })
                .ToListAsync();
            return allClients;
        }
        /// <summary>
        /// This method return all clients for administrator area
        /// </summary>
        /// <returns></returns>
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
                    Address = c.Address,
                    UserId = c.UserId,
                    IsActive = c.IsActive
                })
                .ToListAsync();
            return allClients;
        }

        /// <summary>
        /// This method create client
        /// </summary>
        /// <param name="formModel"></param>
        /// <returns></returns>
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

            await this.userService.AddClaimToUserAsync(client.UserId.ToString(), "FullName",
                $"{client.FirstName} {client.LastName}");
        }
        /// <summary>
        /// This method return returns client details
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<DetailsClientViewModel> DetailsClientAsync(string clientId)
        {
            DetailsClientViewModel client = await this.dbContext
                .Clients
                .Include(c => c.User)
                .Where(c => c.Id.ToString() == clientId && c.IsActive)
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
        /// <summary>
        /// This method check if client exists by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ClientExistsByIdAsync(string id)
        {
            return await this.dbContext
                .Clients
                .AnyAsync(c => c.Id.ToString() == id && c.IsActive);
        }
        /// <summary>
        /// This method return client for edit by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<EditClientFormModel> GetClientForEditByIdAsync(string clientId)
        {
            Client client = await this.dbContext
                .Clients
                .Include(c => c.User)
                .Where(c => c.Id.ToString() == clientId && c.IsActive)
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
        /// <summary>
        /// This method edit client by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
        public async Task EditClientByIdAsync(string clientId, EditClientFormModel formModel)
        {
            Client client = await this.dbContext.Clients
                .Include(c => c.User)
                .Where(c => c.Id.ToString() == clientId && c.IsActive)
                .FirstAsync();
            client.FirstName = formModel.FirstName;
            client.LastName = formModel.LastName;
            client.PhoneNumber = formModel.PhoneNumber;
            client.Address = formModel.Address;
            client.User.Email = formModel.Email;
            await this.dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method return client for delete by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<ClientPreDeleteViewModel> GetClientForPreDeleteByIdAsync(string clientId)
        {
            Client client = await this.dbContext
                .Clients
                .Include(c => c.User)
                .Where(c => c.IsActive)
                .FirstAsync(c => c.Id.ToString() == clientId);
            return new ClientPreDeleteViewModel
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Address = client.Address,
                Email = client.User.Email
            };
        }
        /// <summary>
        /// This method delete client by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task DeleteClientByIdAsync(string clientId)
        {
            Client clientToDelete = await this.dbContext
                .Clients
                .Where(c => c.IsActive)
                .Include(c => c.User)
                .FirstAsync(c => c.Id.ToString() == clientId);
            clientToDelete.IsActive = false;

            Guid userId = clientToDelete.User.Id;
            var userToDelete = await this.userManager.FindByIdAsync(userId.ToString());

            if (userToDelete != null)
            {
                await this.userManager.SetLockoutEnabledAsync(userToDelete, true);
                await this.userManager.SetLockoutEndDateAsync(userToDelete,
                    new System.DateTimeOffset(System.DateTime.Now.AddYears(100)));
            }

            await this.dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method return client id depending on project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<string> GetClientIdByProjectIdAsync(string projectId)
        {
            var project = await this.dbContext
                .Projects
                .Where(x => x.IsActive)
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id.ToString() == projectId);
            if (project == null || project.ClientId.ToString() == string.Empty)
            {
                return null;
            }

            return project.Client.Id.ToString();
        }
        /// <summary>
        /// This method check if client exists by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsClientByUserIdAsync(string userId)
        {
            return await this.dbContext
                .Clients
                .Where(c => c.IsActive)
                .AnyAsync(c => c.UserId.ToString() == userId);
        }
        /// <summary>
        /// This method return client id by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetClientIdByUserIdAsync(string userId)
        {
            return await this.dbContext
                .Clients
                .Where(c => c.IsActive && c.UserId.ToString() == userId)
                .Select(c => c.Id.ToString())
                .FirstAsync();
        }
    }
}