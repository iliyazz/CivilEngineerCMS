//namespace CivilEngineerCMS.Tests.Services
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Threading.Tasks;

//    using CivilEngineerCMS.Common;
//    using CivilEngineerCMS.Data;
//    using CivilEngineerCMS.Web.ViewModels.Administrator;
//    using CivilEngineerCMS.Web.ViewModels.Client;
//    using CivilEngineerCMS.Web.ViewModels.Employee;
//    using CivilEngineerCMS.Web.ViewModels.Manager;
//    using CivilEngineerCMS.Web.ViewModels.Project;

//    using Microsoft.AspNetCore.Identity;

//    using Moq;

//    [TestFixture]
//    public class AdministratorTest
//    {
//        private CivilEngineerCmsDbContext dbContext;
//        private ClientService clientService;
//        private UserService userService;
//        private EmployeeService employeeService;
//        private ProjectService projectService;
//        private Mock<UserManager<ApplicationUser>> userManager;
//        private Mock<RoleManager<IdentityRole<Guid>>> roleManager;
//        public ApplicationUser applicationUser { get; set; }
//        private AdministratorService administratorService;

//        [SetUp]
//        public void SetUp()
//        {
//            dbContext = DatabaseMock.MockDatabase();
//            roleManager = RoleManagerMock.MockRoleManager();
//            userManager = UserManagerMock.MockUserManager();
//            var userManagerMock = new Mock<UserManager<ApplicationUser>>();
//            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
//                .ReturnsAsync(new ApplicationUser()
//                {
//                    Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
//                    Email = "testemail1@email.com",
//                });

//            //userManager = new UserManager<ApplicationUser>(userManagerMock.Object, null, null, null, null, null, null, null, null);
//            userService = new UserService(dbContext, userManager.Object);
//            clientService = new ClientService(dbContext, userManager.Object, userService);
//            employeeService = new EmployeeService(dbContext, userManager.Object, userService);
//            projectService = new ProjectService(dbContext);
//            administratorService = new AdministratorService(dbContext, userManager.Object, roleManager.Object);
//        }

//        [Test]
//        public async Task AllEmployeesForAdministratorAsyncShouldReturnCorrectEmployees()
//        {
//            await using var data = DatabaseMock.MockDatabase();
//            var userManager = UserManagerMock.MockUserManager().Object;

//            //Employee1 info
//            this.applicationUser = new ApplicationUser()
//            {
//                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
//                Email = "TestEmailEmnployee1@mail.com",
//            };
//            await data.Users.AddAsync(applicationUser);

//            var model = new CreateEmployeeFormModel
//            {
//                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
//                FirstName = "TestFirstNameEmployee1",
//                LastName = "TestLastNameEmployee1",
//                PhoneNumber = "+359123456789",
//                Address = "TestAddressEmployee1",
//                JobTitle = "Surveyor1"
//            };
//            employeeService = new EmployeeService(data, userManager, userService);
//            await employeeService.CreateEmployeeAsync(model);
//            var employee1 = data.Employees.FirstOrDefault(x => x.UserId == model.UserId);

//            //this.userManager.IsInRoleAsync(e.User, AdministratorRoleName).GetAwaiter().GetResult(),
//            //this.userManager.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
//            //    .ReturnsAsync(true);
//            this.userManager.
//            employee1.IsActive = true;
//            await data.SaveChangesAsync();

//            //Employee2 info
//            this.applicationUser = new ApplicationUser()
//            {
//                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
//                Email = "TestEmailEmnployee2@mail.com",
//            };
//            await data.Users.AddAsync(applicationUser);

//            model = new CreateEmployeeFormModel
//            {
//                UserId = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
//                FirstName = "TestFirstNameEmployee2",
//                LastName = "TestLastNameEmployee2",
//                PhoneNumber = "+359123456788",
//                Address = "TestAddressEmployee2",
//                JobTitle = "Surveyor2"
//            };
//            employeeService = new EmployeeService(data, userManager, userService);
//            await employeeService.CreateEmployeeAsync(model);
//            var employee2 = data.Employees.FirstOrDefault(x => x.UserId == model.UserId);
//            employee2.IsActive = true;
//            await data.SaveChangesAsync();
//            SelectEmployeesForAdministratorFormModel employee1Model = new SelectEmployeesForAdministratorFormModel
//            {
//                Id = employee1.Id,
//                FirstName = employee1.FirstName,
//                LastName = employee1.LastName,
//                JobTitle = employee1.JobTitle,
//                Email = employee1.User.Email,
//                IsChecked = true,
//                UserId = employee1.UserId
//            };
//            SelectEmployeesForAdministratorFormModel employee2Model = new SelectEmployeesForAdministratorFormModel
//            {
//                Id = employee2.Id,
//                FirstName = employee2.FirstName,
//                LastName = employee2.LastName,
//                JobTitle = employee2.JobTitle,
//                Email = employee2.User.Email,
//                IsChecked = false,
//                UserId = employee2.UserId
//            };

//            Task<IEnumerable<SelectEmployeesForAdministratorFormModel>> resultList = new Task<IEnumerable<SelectEmployeesForAdministratorFormModel>>(() => new List<SelectEmployeesForAdministratorFormModel> { employee1Model, employee2Model });

//            await data.SaveChangesAsync();
//            var result = await administratorService.AllEmployeesForAdministratorAsync();


//            Assert.That(result.Count(), Is.EqualTo(2));
//            //Assert.That(result.First().FirstName, Is.EqualTo(employee1.FirstName));
//            //Assert.That(result.Last().FirstName, Is.EqualTo(employee2.FirstName));
//            //Assert.That(result.First().LastName, Is.EqualTo(employee1.LastName));
//            //Assert.That(result.Last().LastName, Is.EqualTo(employee2.LastName));
//            //Assert.That(result.First().JobTitle, Is.EqualTo(employee1.JobTitle));
//            //Assert.That(result.Last().JobTitle, Is.EqualTo(employee2.JobTitle));
//            //Assert.That(result.First().Email, Is.EqualTo(employee1.User.Email));
//            //Assert.That(result.Last().Email, Is.EqualTo(employee2.User.Email));
//        }

















//        [TearDown]
//        public void TearDown()
//        {
//            dbContext.Dispose();
//        }
//    }
//}
