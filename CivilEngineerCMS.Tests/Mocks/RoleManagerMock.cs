namespace CivilEngineerCMS.Tests.Mocks
{
    using Microsoft.AspNetCore.Identity;

    using Moq;


    public class RoleManagerMock
    {
        public static Mock<RoleManager<IdentityRole<Guid>>> MockRoleManager()
        {
            Mock<RoleManager<IdentityRole<Guid>>> roleManager = new Mock<RoleManager<IdentityRole<Guid>>>(Mock.Of<IRoleStore<IdentityRole<Guid>>>(), null, null, null, null);
            roleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<Guid>());
            roleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole<Guid>());
            roleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole<Guid>>())).ReturnsAsync(IdentityResult.Success);
            roleManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityRole<Guid>>())).ReturnsAsync(IdentityResult.Success);
            roleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole<Guid>>())).ReturnsAsync(IdentityResult.Success);
            return roleManager;





            ////var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            //Mock<UserManager<ApplicationUser>> userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null,
            //    null, null, null);

            //userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

            //userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            //userManager.Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>())).ReturnsAsync(IdentityResult.Success);
            //userManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<Claim>());
            ////return userManager;
            //userManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            //userManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            ////userManager.Setup(um => um.GetUserAsync(
            ////        It.IsAny<ClaimsPrincipal>()))!
            ////    .ReturnsAsync((ClaimsPrincipal principal) =>
            ////        userList.FirstOrDefault(u => u.Id ==
            ////                Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier))));

            //userManager.Setup(um => um.UpdateAsync(
            //        It.IsAny<ApplicationUser>()))
            //    .ReturnsAsync(IdentityResult.Success);

            ////userManager.Setup(um => um.FindByNameAsync(
            ////        It.IsAny<string>()))!
            ////    .ReturnsAsync((string username) =>
            ////        userList.FirstOrDefault(u => u.UserName == username));

            //userManager.Setup(um => um.SetUserNameAsync(
            //        It.IsAny<ApplicationUser>(), It.IsAny<string>()))!
            //    .ReturnsAsync((ApplicationUser user, string newUsername) =>
            //    {
            //        user.UserName = newUsername;
            //        return IdentityResult.Success;
            //    });

            //userManager.Setup(um => um.CheckPasswordAsync(
            //        It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            //    .ReturnsAsync((ApplicationUser user, string givenPassword) =>
            //    {
            //        PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

            //        PasswordVerificationResult result =
            //            hasher.VerifyHashedPassword(user, user.PasswordHash, givenPassword);

            //        return result == PasswordVerificationResult.Success;
            //    });

            //userManager.Setup(um => um.DeleteAsync(
            //        It.IsAny<ApplicationUser>()))
            //    .ReturnsAsync((ApplicationUser user) =>
            //    {
            //        user.UserName = $"DELETED{user.UserName}";

            //        return IdentityResult.Success;
            //    });

            //userManager.Setup(um => um.IsEmailConfirmedAsync(
            //        It.IsAny<ApplicationUser>()))
            //    .ReturnsAsync((ApplicationUser user) => user.EmailConfirmed);

            //userManager.Setup(um => um.GetEmailAsync(
            //        It.IsAny<ApplicationUser>()))
            //    .ReturnsAsync((ApplicationUser user) => user.Email);

            //userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            ////userManager.Setup(um => um.FindByEmailAsync(
            ////        It.IsAny<string>()))!
            ////    .ReturnsAsync((string email) => userList.FirstOrDefault(u => u.Email == email));

            //userManager.Setup(um => um.GetUserIdAsync(
            //        It.IsAny<ApplicationUser>()))
            //    .ReturnsAsync((ApplicationUser user) => user.Id.ToString());

            //userManager.Setup(um => um.GenerateChangeEmailTokenAsync(
            //        It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            //    .ReturnsAsync("random-string");

            //userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(
            //        It.IsAny<ApplicationUser>()))
            //    .ReturnsAsync("random-string");

            //userManager.Setup(um => um.GeneratePasswordResetTokenAsync(
            //        It.IsAny<ApplicationUser>()))
            //    .ReturnsAsync("random-string");

            //userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            ////userManager.Setup(um => um.FindByIdAsync(
            ////        It.IsAny<string>()))!
            ////    .ReturnsAsync((string id) =>
            ////        userList.FirstOrDefault(u => u.Id == Guid.Parse(id)));

            //userManager.Setup(um => um.ChangeEmailAsync(
            //        It.IsAny<ApplicationUser>(),
            //        It.IsAny<string>(),
            //        It.IsAny<string>()))!
            //    .ReturnsAsync((ApplicationUser user, string email, string token) =>
            //    {
            //        user.Email = email;
            //        return IdentityResult.Success;
            //    });

            //userManager.Setup(um => um.ConfirmEmailAsync(
            //        It.IsAny<ApplicationUser>(),
            //        It.IsAny<string>()))!
            //    .ReturnsAsync((ApplicationUser user, string token) =>
            //    {
            //        user.EmailConfirmed = true;
            //        return IdentityResult.Success;
            //    });

            //userManager.Setup(um => um.ChangePasswordAsync(
            //        It.IsAny<ApplicationUser>(),
            //        It.IsAny<string>(),
            //        It.IsAny<string>()))!
            //    .ReturnsAsync((ApplicationUser user, string oldPass, string newPass) =>
            //    {
            //        PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            //        user.PasswordHash = hasher.HashPassword(user, newPass);
            //        return IdentityResult.Success;
            //    });

            //userManager.Setup(um => um.AddToRoleAsync(
            //        It.IsAny<ApplicationUser>(),
            //        It.IsAny<string>()))!
            //    .ReturnsAsync((ApplicationUser user, string role) =>
            //    {
            //        user.UserName = $"{user.UserName}IS-ADMIN";
            //        return IdentityResult.Success;
            //    });

            //userManager.Setup(um => um.RemoveFromRoleAsync(
            //        It.IsAny<ApplicationUser>(),
            //        It.IsAny<string>()))!
            //    .ReturnsAsync((ApplicationUser user, string role) =>
            //    {
            //        user.UserName = $"{user.UserName}IS-NOT-ADMIN";
            //        return IdentityResult.Success;
            //    });

            //userManager.Setup(um => um.CreateAsync(
            //        It.IsAny<ApplicationUser>(),
            //        It.IsAny<string>()))!
            //    .ReturnsAsync((ApplicationUser user, string password) =>
            //    {
            //        PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            //        string hash = hasher.HashPassword(user, password);

            //        user.PasswordHash = hash;
            //        userList.Add(user);

            //        return IdentityResult.Success;
            //    });

            //userManager.SetupGet(um => um.Users)
            //    .Returns(userList.AsQueryable());

            //return userManager;
        }
    }
}