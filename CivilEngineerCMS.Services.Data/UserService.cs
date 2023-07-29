namespace CivilEngineerCMS.Services.Data
{
    using System.Security.Claims;

    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Data.Models;

    using Interfaces;

    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly CivilEngineerCmsDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(CivilEngineerCmsDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task AddClaimToUserAsync(string userId, string claimType, string claimValue)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            await this.userManager.AddClaimAsync(user, new Claim(claimType, claimValue));
        }

        public async Task<Claim?> GetClaimValueByUserIdAsync(string userId, string claimType)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            Claim? claim = (await this.userManager.GetClaimsAsync(user)).FirstOrDefault(x => x.Type == claimType);
            return claim;
        }
    }
}