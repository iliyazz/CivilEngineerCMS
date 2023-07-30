﻿namespace CivilEngineerCMS.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CivilEngineerCMS.Common;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Services.Data;
    using CivilEngineerCMS.Web.ViewModels.Administrator;
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Employee;
    using CivilEngineerCMS.Web.ViewModels.Manager;
    using CivilEngineerCMS.Web.ViewModels.Project;

    using Microsoft.AspNetCore.Identity;

    using Moq;

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

            //userManager = new UserManager<ApplicationUser>(userManagerMock.Object, null, null, null, null, null, null, null, null);
            userService = new UserService(dbContext, userManager.Object);
            clientService = new ClientService(dbContext, userManager.Object, userService);
            employeeService = new EmployeeService(dbContext, userManager.Object, userService);
            projectService = new ProjectService(dbContext);
            administratorService = new AdministratorService(dbContext, userManager.Object, roleManager.Object);
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

            administratorService = new AdministratorService(data, userManager, roleManager.Object);

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

            administratorService = new AdministratorService(data, userManager, roleManager.Object);

            var res2 = await administratorService.AllEmployeesForAdministratorAsync();

            Assert.That(res2.Count, Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
