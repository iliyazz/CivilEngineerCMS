namespace CivilEngineerCMS.Services.Data.Interfaces
{
    using System.Security.Claims;

    public interface IUserService
    {
        Task AddClaimToUserAsync(string userId, string claimType, string claimValue);
        Task<Claim?> GetClaimValueByUserIdAsync(string userId, string claimType);
    }
}
