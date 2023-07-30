namespace CivilEngineerCMS.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CivilEngineerCMS.Common;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Services.Data.Models.Project;
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Employee;
    using CivilEngineerCMS.Web.ViewModels.Project;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using Web.ViewModels.Project.Enums;

    using Project = Data.Models.Project;

    [TestFixture]
    public class HomeTest
    {
        private CivilEngineerCmsDbContext dbContext;
        private ClientService clientService;
        private UserService userService;
        private EmployeeService employeeService;
        private ProjectService projectService;
        private HomeService homeService;
        private Mock<UserManager<ApplicationUser>> userManager;
        public ApplicationUser applicationUser { get; set; }

        [SetUp]
        public void SetUp()
        {
            dbContext = DatabaseMock.MockDatabase();
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
            homeService = new HomeService(dbContext);
        }



        [Test]
        public async Task AllIndexProjectsAsyncShouldReturnCorrectValue()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
            await data.Users.AddAsync(applicationUser);

            var modelClient = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstNameClient1",
                LastName = "TestLastNameClient1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressClient1",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(modelClient);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;

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

            //Project1 info
            Guid managerId1 = employee.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
                UrlPicturePath = "TestUrl1",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault();
            project1.IsActive = true;

            //Project2 info
            var managerId2 = employee.Id;
            clientId = client.Id;
            var modelProject2 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName2",
                Description = "TestProjectDescription2",
                ClientId = clientId,
                ManagerId = managerId2,
                Status = (ProjectStatusEnums)2,
                ProjectEndDate = "15.12.2024",
                UrlPicturePath = "TestUrl2",

            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject2);
            var project2 = data.Projects.FirstOrDefault();
            project2.IsActive = true;

            await data.SaveChangesAsync();

            homeService = new HomeService(data);

            var res = await this.homeService.AllIndexProjectsAsync();

            Assert.That(res.Count, Is.EqualTo(2));

        }



        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
