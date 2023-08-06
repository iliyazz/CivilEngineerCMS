namespace CivilEngineerCMS.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Services.Data;
    using CivilEngineerCMS.Web.ViewModels.Administrator;
    using CivilEngineerCMS.Web.ViewModels.Employee;

    using Microsoft.AspNetCore.Identity;

    using Moq;
    using Employee = Data.Models.Employee;


    [TestFixture]
    public class AdministratorTest
    {
        private CivilEngineerCmsDbContext dbContext;
        private ClientService clientService;
        private UserService userService;
        private EmployeeService employeeService;
        private ProjectService projectService;
        private AdministratorService administratorService;
        private HomeService homeService;

        private Mock<UserManager<ApplicationUser>> userManager;
        private Mock<RoleManager<IdentityRole<Guid>>> roleManager;
        public ApplicationUser applicationUser { get; set; }

        [SetUp]
        public void SetUp()
        {
            dbContext = DatabaseMock.MockDatabase();
            roleManager = RoleManagerMock.MockRoleManager();
            userManager = UserManagerMock.MockUserManager();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser()
                {
                    Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                    Email = "testemail1@email.com",
                });

            userService = new UserService(dbContext, userManager.Object);
            clientService = new ClientService(dbContext, userManager.Object, userService);
            employeeService = new EmployeeService(dbContext, userManager.Object, userService);
            projectService = new ProjectService(dbContext);
            administratorService = new AdministratorService(dbContext, userManager.Object);
            homeService = new HomeService(dbContext);
        }

        [Test]
        public async Task AllIndexProjectsAsyncShouldReturnCorrectValue()
        {
            var userManager = UserManagerMock.MockUserManager().Object;

            await using var data = DatabaseMock.MockDatabase();

            //Employee info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee);
            var employee = data.Employees.FirstOrDefault();
            employee.IsActive = true;


            await data.SaveChangesAsync();

            administratorService = new AdministratorService(data, userManager);

            var res2 = await administratorService.AllEmployeesForAdministratorAsync();

            Assert.That(res2.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task AllIndexProjectsAsyncShouldReturnEmptyList()
        {
            var userManager = UserManagerMock.MockUserManager().Object;

            await using var data = DatabaseMock.MockDatabase();

            //Employee info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee);
            var employee = data.Employees.FirstOrDefault();
            employee.IsActive = false;


            await data.SaveChangesAsync();

            administratorService = new AdministratorService(data, userManager);

            var res2 = await administratorService.AllEmployeesForAdministratorAsync();

            Assert.That(res2.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task SeedDataShouldSeedData()
        {
            var userManager = UserManagerMock.MockUserManager().Object;

            await using var data = DatabaseMock.MockDatabase();

            //Employee info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "abv@abv.bg"
            };
            await data.Users.AddAsync(applicationUser);
            await data.SaveChangesAsync();

            var resultUser = await data.Users.FirstOrDefaultAsync(u => u.Email == "abv@abv.bg");
            Assert.That(resultUser, Is.Not.Null);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
