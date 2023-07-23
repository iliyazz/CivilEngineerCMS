namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasData(GenerateApplicationUser());
        }

        private ApplicationUser[] GenerateApplicationUser()
        {
            ICollection<ApplicationUser> applicationUsers = new HashSet<ApplicationUser>();
            ApplicationUser applicationUser;
            applicationUser = new ApplicationUser
            {
                Id = Guid.Parse("60376974-E414-4277-1D75-08DB7A21C396"),
                UserName = "iliyaz.softuni@gmail.com",
                NormalizedUserName = "ILIYAZ.SOFTUNI@GMAIL.COM",
                Email = "iliyaz.softuni@gmail.com",
                NormalizedEmail = "ILIYAZ.SOFTUNI@GMAIL.COM",
                EmailConfirmed = true,
                AccessFailedCount = 0,
                SecurityStamp = "3B0A598F-AA09-4D3B-B0DE-45C0F5938332",
                PhoneNumber = "+359123456789",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
            applicationUsers.Add(applicationUser);

            return applicationUsers.ToArray();
        }
    }
}
