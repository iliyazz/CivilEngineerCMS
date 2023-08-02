


namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        //UserManager<ApplicationUser> userManager;

        //public ApplicationUserEntityConfiguration(UserManager<ApplicationUser> userManager)
        //{
        //    userManager = userManager;
        //}

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

        //private ApplicationUser[] GenerateApplicationUser()
        //{
        //    ICollection<ApplicationUser> applicationUsers = new HashSet<ApplicationUser>();
        //    PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

        //    ApplicationUser applicationUser;




        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("B85227A0-4FB4-4B5E-84AD-08DB807C5EDB"),
        //        UserName = "iliyaz.softuni@gmail.com",
        //        NormalizedUserName = "ILIYAZ.SOFTUNI@GMAIL.COM",
        //        Email = "iliyaz.softuni@gmail.com",
        //        NormalizedEmail = "ILIYAZ.SOFTUNI@GMAIL.COM",
        //        EmailConfirmed = false,
        //        SecurityStamp = "LJW7J33EVBQOCAMXUDU6OLJIC5NFDMBG",
        //        ConcurrencyStamp = "84d353ba-b317-4b4f-a999-7067b03cf4c4",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Iliya Zapryanov"));
        //    applicationUsers.Add(applicationUser);

        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("09BEAA1D-0E57-4481-84AE-08DB807C5EDB"),
        //        UserName = "client1@abv.bg",
        //        NormalizedUserName = "CLIENT1@ABV.BG",
        //        Email = "client1@abv.bg",
        //        NormalizedEmail = "CLIENT1@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "EGBE7MW3IQ62TDS2UB6RTLZWYKRGI7HK",
        //        ConcurrencyStamp = "0de36425-92a7-4fa2-ac8b-766543dd5292",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Misho Mishev"));

        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("B19FA0F8-35C1-403E-84AF-08DB807C5EDB"),
        //        UserName = "client2@abv.bg",
        //        NormalizedUserName = "CLIENT2@ABV.BG",
        //        Email = "client2@abv.bg",
        //        NormalizedEmail = "CLIENT2@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "L34535Q36KOO2DOOEKIHAML6QXLZMWXG",
        //        ConcurrencyStamp = "2f153102-3c22-4abb-9d56-56e782504593",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Tosho Toshev"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("1E7D4F19-E822-4630-84B0-08DB807C5EDB"),
        //        UserName = "employee1@abv.bg",
        //        NormalizedUserName = "EMPLOYEE1@ABV.BG",
        //        Email = "employee1@abv.bg",
        //        NormalizedEmail = "EMPLOYEE1@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "DAQB6US2MLHDYAH5Q44H4EG56WZQWTJW",
        //        ConcurrencyStamp = "8da4ebd6-edb2-4ba2-a8e3-fef56521ad51",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Ivan Ivanov"));
        //    this.userManager.AddToRoleAsync(applicationUser, AdministratorRoleName);
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("669FEF1E-980F-41B0-84B1-08DB807C5EDB"),
        //        UserName = "employee2@abv.bg",
        //        NormalizedUserName = "EMPLOYEE2@ABV.BG",
        //        Email = "employee2@abv.bg",
        //        NormalizedEmail = "EMPLOYEE2@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "KQVDXRUWQYJRKLU4TNKAS2OYI7ZF3EQI",
        //        ConcurrencyStamp = "65a49896-decd-4a92-ba7a-215b183a5f70",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Iliya Iliyev"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("3310B8C2-4D50-4FDA-570C-08DB823D71D7"),
        //        UserName = "client3@abv.bg",
        //        NormalizedUserName = "CLIENT3@ABV.BG",
        //        Email = "client3@abv.bg",
        //        NormalizedEmail = "CLIENT3@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "7TWUT6SXEEPT33WNPMH3B4WUQQ65LNF4",
        //        ConcurrencyStamp = "57e6d246-2531-4d26-a433-b2860024e04d",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Georgi Georgiev"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("811CD974-5562-4D33-B491-08DB82505C39"),
        //        UserName = "employee3@abv.bg",
        //        NormalizedUserName = "EMPLOYEE3@ABV.BG",
        //        Email = "employee3@abv.bg",
        //        NormalizedEmail = "EMPLOYEE3@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "H76BE4FJRA465FU34FXI4WZH54D4XRPO",
        //        ConcurrencyStamp = "c68671dd-d1c7-45e0-b813-a14644907460",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Gergana Gerganova-employee3"));
        //    this.userManager.AddToRoleAsync(applicationUser, AdministratorRoleName);
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("6945C1FD-9E01-4F68-48FA-08DB86CAD14A"),
        //        UserName = "client4@abv.bg",
        //        NormalizedUserName = "CLIENT4@ABV.BG",
        //        Email = "client4@abv.bg",
        //        NormalizedEmail = "CLIENT4@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "WPNF3TLDGZVUIOU5P53UTS6V4XSH2QU6",
        //        ConcurrencyStamp = "e3ad5022-736a-45d4-8faf-57be74d9a69e",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Tencho Tenchev"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("A3C05D98-F2A8-45F3-48FB-08DB86CAD14A"),
        //        UserName = "client5@abv.bg",
        //        NormalizedUserName = "CLIENT5@ABV.BG",
        //        Email = "client5@abv.bg",
        //        NormalizedEmail = "CLIENT5@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "7J5FGZI2FDRSSCPRVWXWDXD54SJTEMQZ",
        //        ConcurrencyStamp = "0a7279ff-07d0-405d-8a95-647c1f55d19a",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = DateTimeOffset.Parse("2123-07-25 20:44:51.4000030 +03:00", CultureInfo.InvariantCulture, DateTimeStyles.None),
        //        LockoutEnabled = true,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Angel Angelov"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("5B8F3E1A-D68B-4E89-48FC-08DB86CAD14A"),
        //        UserName = "employee4@abv.bg",
        //        NormalizedUserName = "EMPLOYEE4@ABV.BG",
        //        Email = "employee4@abv.bg",
        //        NormalizedEmail = "EMPLOYEE4@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "YLQLYDOVYDYGZO5BUOYUBOCPPQQGFBKG",
        //        ConcurrencyStamp = "7f784d88-3f01-4d11-a54c-ff14de67e66a",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Marina Marinova"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("1039301D-024F-4F1A-48FD-08DB86CAD14A"),
        //        UserName = "employee5@abv.bg",
        //        NormalizedUserName = "EMPLOYEE5@ABV.BG",
        //        Email = "employee5@abv.bg",
        //        NormalizedEmail = "EMPLOYEE5@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "KA46RD4HDSXICHX4XQTYGNAJEYR64D63",
        //        ConcurrencyStamp = "8b1a77c1-35ed-45d1-9d80-945000e31e89",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Decho Dechev"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        //
        //        Id = Guid.Parse("42C8AA2F-AF8F-42B2-B69E-08DB88752865"),
        //        UserName = "client7@abv.bg",
        //        NormalizedUserName = "CLIENT7@ABV.BG",
        //        Email = "client7@abv.bg",
        //        NormalizedEmail = "CLIENT7@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "5WIAOI5QCUJUVLN5AQ3RMWSG62OHCVOX",
        //        ConcurrencyStamp = "8fe5fd36-1268-4a7a-91cd-11e939f93f98",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Iordanka Iordanova"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("4FA32242-0E4F-406E-A7C9-37BA26018B8F"),
        //        UserName = "employee6@abv.bg",
        //        NormalizedUserName = "EMPLOYEE6@ABV.BG",
        //        Email = "employee6@abv.bg",
        //        NormalizedEmail = "EMPLOYEE6@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "XMELJ66XTLOCCWWZBSON5EAHCV553733",
        //        ConcurrencyStamp = "1a7ca707-5475-4360-9696-f30e55724e61",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Nedialka Nedialkova"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("987AABEE-61D7-4699-8E34-46A1BD1569BD"),
        //        UserName = "forRecapture@abv.bg",
        //        NormalizedUserName = "FORRECAPTURE@ABV.BG",
        //        Email = "forRecapture@abv.bg",
        //        NormalizedEmail = "FORRECAPTURE@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "2WJUIFMJ5WGVAX37RVBQJPFDDMJ3V6U6",
        //        ConcurrencyStamp = "347d497d-73d7-48fa-b85d-9aa97251d04a",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("0E7115FA-D727-4A28-B2AD-62F815F2D899"),
        //        UserName = "employee7@abv.bg",
        //        NormalizedUserName = "EMPLOYEE7@ABV.BG",
        //        Email = "employee7@abv.bg",
        //        NormalizedEmail = "EMPLOYEE7@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "QFQ72R5XHZO32GCB4CT66IP7XJC2DAOB",
        //        ConcurrencyStamp = "327ce451-9688-44a6-bc6b-099995849277",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    this.userManager.AddClaimAsync(applicationUser, new Claim("FullName", "Krastio Krastev"));
        //    applicationUsers.Add(applicationUser);
        //    applicationUser = new ApplicationUser
        //    {
        //        Id = Guid.Parse("75CD4FDE-0948-4CFE-8A22-AD2DCDC9001A"),
        //        UserName = "client8@abv.bg",
        //        NormalizedUserName = "CLIENT8@ABV.BG",
        //        Email = "client8@abv.bg",
        //        NormalizedEmail = "CLIENT8@ABV.BG",
        //        EmailConfirmed = false,
        //        SecurityStamp = "EY6CJ5BAEQLPHFK2ISVQ6P23TEGWIBXK",
        //        ConcurrencyStamp = "0045340b-a85e-4db0-9b65-8dae03c56c63",
        //        PhoneNumber = null,
        //        PhoneNumberConfirmed = false,
        //        TwoFactorEnabled = false,
        //        LockoutEnd = null,
        //        LockoutEnabled = false,
        //        AccessFailedCount = 0,
        //    };
        //    applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "123456");
        //    applicationUsers.Add(applicationUser);






        //    return applicationUsers.ToArray();
        //}
    }
}
