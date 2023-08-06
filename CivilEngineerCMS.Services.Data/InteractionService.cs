using CivilEngineerCMS.Services.Data.Interfaces;

namespace CivilEngineerCMS.Services.Data
{
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Web.ViewModels.Interaction;

    using Task = Task;

    public class InteractionService : IInteractionService
    {
        private readonly CivilEngineerCmsDbContext dbContext;

        public InteractionService(CivilEngineerCmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        /// <summary>
        /// This method return all interactions by project id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AddAndEditInteractionFormModel>> AllInteractionsByProjectIdAsync(string id)
        {
            IEnumerable<AddAndEditInteractionFormModel> allInteractionsByProjectIdIdAsync = await dbContext
                .Interactions
                .Where(i => i.ProjectId.ToString() == id)
                .OrderBy(x => x.Date)
                .Select(i => new AddAndEditInteractionFormModel
                {
                    Id = i.Id,
                    Date = i.Date,
                    Description = i.Description,
                    Message = i.Message,
                    UrlPath = i.UrlPath,
                    ProjectId = i.ProjectId,
                    Type = i.Type,
                })
                .ToListAsync();
            return allInteractionsByProjectIdIdAsync;
        }
        /// <summary>
        /// This method check if interaction exists by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<bool> InteractionExistsByProjectIdAsync(string projectId)
        {
            return dbContext.Interactions.AnyAsync(i => i.ProjectId.ToString() == projectId);
        }
        /// <summary>
        /// This method create interaction by project id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
        public async Task CreateInteractionAsync(string id, AddAndEditInteractionFormModel formModel)
        {
            Interaction interaction = new Interaction
            {
                ProjectId = Guid.Parse(id),
                Date = formModel.Date,
                Description = formModel.Description,
                Message = formModel.Message,
                UrlPath = formModel.UrlPath,
                Type = formModel.Type,
            };
            await dbContext.Interactions.AddAsync(interaction);
            await dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// This method return interaction for edit by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="interactionId"></param>
        /// <returns></returns>
        public async Task<AddAndEditInteractionFormModel> GetInteractionForEditByProjectIdAsync(string projectId, string interactionId)
        {
            Interaction interaction = await dbContext.Interactions
                .Where(i => i.ProjectId.ToString() == projectId && i.Id.ToString() == interactionId)
                .FirstAsync();
            AddAndEditInteractionFormModel result = new AddAndEditInteractionFormModel
            {
                Id = interaction.Id,
                ProjectId = interaction.ProjectId,
                Type = interaction.Type,
                Description = interaction.Description,
                Message = interaction.Message,
                UrlPath = interaction.UrlPath,
                Date = interaction.Date,
            };
            return result;
        }
        /// <summary>
        /// This method Edit interaction by project id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formModel"></param>
        /// <returns></returns>
        public async Task EditInteractionByProjectIdAsync(string id, AddAndEditInteractionFormModel formModel)
        {
            Interaction interaction = await this.dbContext
                .Interactions
                .Where(i => i.Id.ToString() == id)
                .FirstAsync();
            interaction.Type = formModel.Type;
            interaction.Description = formModel.Description;
            interaction.Message = formModel.Message;
            interaction.UrlPath = formModel.UrlPath;
            interaction.Date = DateTime.UtcNow;
            await this.dbContext.SaveChangesAsync();
        }
    }
}

