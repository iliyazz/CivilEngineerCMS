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
                Id = Guid.Parse("B85227A0-4FB4-4B5E-84AD-08DB807C5EDB"),
                UserName = "iliyaz.softuni@gmail.com",
                NormalizedUserName = "ILIYAZ.SOFTUNI@GMAIL.COM",
                Email = "iliyaz.softuni@gmail.com",
                NormalizedEmail = "ILIYAZ.SOFTUNI@GMAIL.COM",
                EmailConfirmed = true,
                AccessFailedCount = 0,
                SecurityStamp = "LJW7J33EVBQOCAMXUDU6OLJIC5NFDMBG",
                ConcurrencyStamp = "31ca9820-e6fd-48dd-8483-68225bd2071c",
                PhoneNumber = "+359123456789",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
            applicationUsers.Add(applicationUser);

            return applicationUsers.ToArray();
        }
    }
}
