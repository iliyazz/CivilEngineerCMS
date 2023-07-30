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
    using CivilEngineerCMS.Web.ViewModels.Manager;
    using CivilEngineerCMS.Web.ViewModels.Project;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Query.Internal;
    using Moq;
    using Web.ViewModels.Project.Enums;
    using Project = Data.Models.Project;

    [TestFixture]
    public class ProjectTest
    {
        private CivilEngineerCmsDbContext dbContext;
        private ClientService clientService;
        private UserService userService;
        private EmployeeService employeeService;
        private ProjectService projectService;
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
        }

        [Test]
        public Task StatusExistsShouldReturnTrue()
        {
            bool resultTrue = projectService.StatusExists("NotYetStarted");
            bool resultFalse = projectService.StatusExists("Incorrect");

            Assert.IsTrue(resultTrue);
            Assert.IsFalse(resultFalse);
            return Task.CompletedTask;
        }

        [Test]
        public async Task CreateProjectAsyncShouldReturnCreateCorrectProject()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailEmployee1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var model = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor1"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(model);
            var employee1 = data.Employees.FirstOrDefault(x => x.UserId == model.UserId);
            employee1.IsActive = true;
            await data.SaveChangesAsync();
            var employee1Id = employee1.Id;


            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                Email = "TestEmailEmployee2@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            model = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                FirstName = "TestFirstNameEmployee2",
                LastName = "TestLastNameEmployee2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee2",
                JobTitle = "Surveyor2"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(model);
            var employee2 = data.Employees.FirstOrDefault(x => x.UserId == model.UserId);
            employee2.IsActive = true;
            await data.SaveChangesAsync();
            var employee2Id = employee2.Id;


            //Client1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("9BE16B11-C343-467E-A88C-D1F2E47085C6"),
                Email = "TestEmailClient1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelClient = new CreateClientFormModel
            {
                UserId = Guid.Parse("9BE16B11-C343-467E-A88C-D1F2E47085C6"),
                FirstName = "TestFirstNameClient1",
                LastName = "TestLastNameClient1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressClient1",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(modelClient);
            await data.SaveChangesAsync();

            var client1 = data.Clients.FirstOrDefault(x => x.UserId == modelClient.UserId);
            client1.IsActive = true;

            //Project1 info
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = client1.Id,
                ManagerId = employee1Id,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "2023-12-15",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project1 = data.Projects.FirstOrDefault(x => x.Name == "TestProjectName1");
            project1.IsActive = true;
            project1.ProjectCreatedDate = DateTime.Parse("2023-12-15");
            await data.SaveChangesAsync();
            var project1Id = project1.Id;

            List<ProjectEmployee> idList = new List<ProjectEmployee>()
            {
                new()
                {
                    EmployeeId = employee1Id,
                    ProjectId = project1Id
                },
                new()
                {
                    EmployeeId = employee2Id,
                    ProjectId = project1Id
                }
            };
            await data.ProjectsEmployees.AddRangeAsync(idList);
            await data.SaveChangesAsync();

            //IEnumerable<AllEmployeeViewModel> result = await employeeService.AllEmployeesByProjectIdAsync(project1Id.ToString());
            var result = await data.Projects.CountAsync();

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task IsManagerOfProjectAsyncShouldReturnCorrectValue()
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "2023-12-15",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            await data.SaveChangesAsync();


            string projectId = project.Id.ToString();
            string clientIdExpected = modelProject.ClientId.ToString();
            bool clientIdActualTrue = await projectService.IsManagerOfProjectAsync(projectId, managerId.ToString());
            bool clientIdActualFalse = await projectService.IsManagerOfProjectAsync(projectId, clientId.ToString());

            Assert.IsTrue(clientIdActualTrue);
            Assert.IsFalse(clientIdActualFalse);
        }

        [Test]
        public async Task GetProjectForEditByIdAsyncShouldReturnCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            await data.SaveChangesAsync();

            var result = await projectService.GetProjectForEditByIdAsync(project.Id.ToString());

            Assert.That(result.Name, Is.EqualTo(modelProject.Name));
            Assert.That(result.Description, Is.EqualTo(modelProject.Description));
            Assert.That(result.ClientId, Is.EqualTo(modelProject.ClientId));
            Assert.That(result.ManagerId, Is.EqualTo(modelProject.ManagerId));
            Assert.That(result.Status, Is.EqualTo(modelProject.Status));
            Assert.That(result.ProjectEndDate, Is.EqualTo(modelProject.ProjectEndDate));
        }

        [Test]
        public async Task EditProjectByIdAsyncShouldReturnCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;

            await data.SaveChangesAsync();

            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName3",
                Description = "TestProjectDescription3",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            await projectService.EditProjectByIdAsync(project.Id.ToString(), modelProject1);



            var result = await data.Projects.FirstOrDefaultAsync(p => p.Name == "TestProjectName3");

            Assert.That(result.Name, Is.EqualTo(modelProject1.Name));
            Assert.That(result.Description, Is.EqualTo(modelProject1.Description));
            Assert.That(result.ClientId, Is.EqualTo(modelProject1.ClientId));
            Assert.That(result.ManagerId, Is.EqualTo(modelProject1.ManagerId));
            Assert.That(result.Status, Is.EqualTo(modelProject1.Status));
            Assert.That(result.ProjectEndDate.ToString("dd.MM.yyyy"), Is.EqualTo(modelProject1.ProjectEndDate));
        }

        [Test]
        public async Task DetailsByIdProjectAsyncShouldReturnCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            var projectStartDate = project.ProjectCreatedDate;
            var projectManagerName = project.Manager.FirstName + " " + project.Manager.LastName;
            await data.SaveChangesAsync();

            var result = await projectService.DetailsByIdProjectAsync(project.Id.ToString());

            Assert.That(result.Name, Is.EqualTo(modelProject.Name));
            Assert.That(result.Description, Is.EqualTo(modelProject.Description));
            Assert.That(result.UrlPicturePath, Is.EqualTo(modelProject.UrlPicturePath));
            Assert.That(result.Status, Is.EqualTo(modelProject.Status));
            Assert.That(result.ProjectStartDate, Is.EqualTo(projectStartDate.ToString("dd.MM.yyyy")));
            Assert.That(result.ProjectEndDate, Is.EqualTo(modelProject.ProjectEndDate));
            Assert.That(result.ManagerName, Is.EqualTo(projectManagerName));
            Assert.That(result.ClientName, Is.EqualTo(client.FirstName + " " + client.LastName));
            Assert.That(result.ClientPhone, Is.EqualTo(client.PhoneNumber));
            Assert.That(result.ClientEmail, Is.EqualTo(client.User.Email));
        }

        [Test]
        public async Task ProjectExistsByIdAsyncShouldReturnTrueIfProjecttExists()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            var projectStartDate = project.ProjectCreatedDate;
            var projectManagerName = project.Manager.FirstName + " " + project.Manager.LastName;
            await data.SaveChangesAsync();

            var result = await projectService.ProjectExistsByIdAsync(project.Id.ToString());
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ProjectExistsByIdAsyncShouldReturnFalseIfProjectNotExists()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            var result = await projectService.ProjectExistsByIdAsync("1FA37C21-89E7-4F63-853A-43BC8B5B6504");
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetProjectForPreDeleteByIdAsyncShouldReturnCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            var projectStartDate = project.ProjectCreatedDate;
            var projectManagerName = project.Manager.FirstName + " " + project.Manager.LastName;
            await data.SaveChangesAsync();

            var result = await projectService.GetProjectForPreDeleteByIdAsync(project.Id.ToString());

            Assert.That(result.Name, Is.EqualTo(modelProject.Name));
            Assert.That(result.Description, Is.EqualTo(modelProject.Description));
            Assert.That(result.ClientName, Is.EqualTo(client.FirstName + " " + client.LastName));
            Assert.That(result.ManagerName, Is.EqualTo(projectManagerName));
            Assert.That(result.ProjectStartDate, Is.EqualTo(projectStartDate.ToString("dd.MM.yyyy")));
            Assert.That(result.ProjectEndDate, Is.EqualTo(modelProject.ProjectEndDate));
            Assert.That(result.Status, Is.EqualTo(modelProject.Status));
        }

        [Test]
        public async Task DeleteProjectByIdAsyncShouldDeleteProject()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            var projectStartDate = project.ProjectCreatedDate;
            var projectManagerName = project.Manager.FirstName + " " + project.Manager.LastName;
            await data.SaveChangesAsync();
            
            bool isDeletedBeforeAction = await projectService.ProjectExistsByIdAsync(project.Id.ToString());
            await projectService.DeleteProjectByIdAsync(project.Id.ToString());
            bool isDeletedAfterAction = await projectService.ProjectExistsByIdAsync(project.Id.ToString());

            Assert.That(isDeletedBeforeAction, Is.True);
            Assert.That(isDeletedAfterAction, Is.False);
        }

        [Test]
        public async Task ProjectAllFilteredAndPagedAsyncShouldReturnCorrectDataWhenStatusIsNullOrWhiteSpace()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault();
            employee1.IsActive = true;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault();
            employee2.IsActive = true;


            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault();
            project1.IsActive = true;
            var projectStartDate1 = project1.ProjectCreatedDate;
            var projectManagerName1 = project1.Manager.FirstName + " " + project1.Manager.LastName;

            //Project2 info
            var managerId2 = employee2.Id;
            clientId = client.Id;
            var modelProject2 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName2",
                Description = "TestProjectDescription2",
                ClientId = clientId,
                ManagerId = managerId2,
                Status = (ProjectStatusEnums)2,
                ProjectEndDate = "15.12.2024",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject2);
            var project2 = data.Projects.FirstOrDefault();
            project2.IsActive = true;
            var projectStartDate2 = project1.ProjectCreatedDate;
            var projectManagerName2 = project1.Manager.FirstName + " " + project1.Manager.LastName;

            await data.SaveChangesAsync();

            ProjectAllQueryModel projectAllQueryModel = new ProjectAllQueryModel
            {
                Status = null,
                SearchString = "Description1",
                ProjectSorting = ProjectSorting.ProjectName,
                CurrentPage = 0,
                ProjectsPerPage = 0,
                TotalProjects = 0,
            };

            List<Project> projects = new List<Project>
            {
                project1,
                project2
            };

            ProjectAllFilteredAndPagedServiceModel serviceModel =
                await this.projectService.ProjectAllFilteredAndPagedAsync(projectAllQueryModel);
            projectAllQueryModel.Projects = serviceModel.Projects;
            projectAllQueryModel.TotalProjects = serviceModel.TotalProjectsCount;
            projectAllQueryModel.Statuses = Enum.GetNames(typeof(ProjectStatusEnums)).ToList();

            Assert.That(projectAllQueryModel.TotalProjects, Is.EqualTo(1));


        }

        [Test]
        public async Task ProjectAllFilteredAndPagedAsyncShouldReturnCorrectDataWhenStatusIsNotNullOrWhiteSpace()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault();
            employee1.IsActive = true;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault();
            employee2.IsActive = true;


            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault();
            project1.IsActive = true;
            var projectStartDate1 = project1.ProjectCreatedDate;
            var projectManagerName1 = project1.Manager.FirstName + " " + project1.Manager.LastName;

            //Project2 info
            var managerId2 = employee2.Id;
            clientId = client.Id;
            var modelProject2 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName2",
                Description = "TestProjectDescription2",
                ClientId = clientId,
                ManagerId = managerId2,
                Status = (ProjectStatusEnums)2,
                ProjectEndDate = "15.12.2024",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject2);
            var project2 = data.Projects.FirstOrDefault();
            project2.IsActive = true;
            var projectStartDate2 = project1.ProjectCreatedDate;
            var projectManagerName2 = project1.Manager.FirstName + " " + project1.Manager.LastName;

            await data.SaveChangesAsync();

            ProjectAllQueryModel projectAllQueryModel = new ProjectAllQueryModel
            {
                Status = (ProjectStatusEnums)1,
                SearchString = "Description1",
                ProjectSorting = ProjectSorting.ProjectName,
                CurrentPage = 0,
                ProjectsPerPage = 0,
                TotalProjects = 0,
            };

            List<Project> projects = new List<Project>
            {
                project1,
                project2
            };

            ProjectAllFilteredAndPagedServiceModel serviceModel =
                await this.projectService.ProjectAllFilteredAndPagedAsync(projectAllQueryModel);
            projectAllQueryModel.Projects = serviceModel.Projects;
            projectAllQueryModel.TotalProjects = serviceModel.TotalProjectsCount;
            projectAllQueryModel.Statuses = Enum.GetNames(typeof(ProjectStatusEnums)).ToList();

            Assert.That(projectAllQueryModel.TotalProjects, Is.EqualTo(1));


        }

        [Test]
        public async Task GetStatisticsAsyncShouldReturnCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault();
            employee1.IsActive = true;

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault();
            employee2.IsActive = true;


            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName1");
            project1.IsActive = true;

            //Project2 info
            var managerId2 = employee2.Id;
            clientId = client.Id;
            var modelProject2 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName2",
                Description = "TestProjectDescription2",
                ClientId = clientId,
                ManagerId = managerId2,
                Status = (ProjectStatusEnums)2,
                ProjectEndDate = "15.12.2024",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject2);
            var project2 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName2");
            project2.IsActive = true;

            //Project3 info
            var managerId3 = employee2.Id;
            clientId = client.Id;
            var modelProject3 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName3",
                Description = "TestProjectDescription3",
                ClientId = clientId,
                ManagerId = managerId2,
                Status = (ProjectStatusEnums)3,
                ProjectEndDate = "15.12.2024",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject3);
            var project3 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName3");
            project3.IsActive = false;

            await data.SaveChangesAsync();

            var projectStatistics = await projectService.GetStatisticsAsync();

            Assert.That(projectStatistics.TotalProjects, Is.EqualTo(3));
            Assert.That(projectStatistics.TotalClients, Is.EqualTo(1));
            Assert.That(projectStatistics.TotalActiveProjects, Is.EqualTo(2));
        }

        [Test]
        public async Task AllEmployeesForProjectAsyncShouldReturnCorrectDataWhenAllEmployeeAreActive()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee1");
            employee1.IsActive = true;
            Guid employee1Id = employee1.Id;

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee2",
                LastName = "TestLastNameEmployee2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee2",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee2");
            employee2.IsActive = true;
            Guid employee2Id = employee2.Id;


            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName1");
            project1.IsActive = true;
            Guid project1Id = project1.Id;

            List<ProjectEmployee> idList = new List<ProjectEmployee>()
            {
                new()
                {
                    EmployeeId = employee1Id,
                    ProjectId = project1Id
                },
                new()
                {
                    EmployeeId = employee2Id,
                    ProjectId = project1Id
                }
            };
            await data.ProjectsEmployees.AddRangeAsync(idList);

            await data.SaveChangesAsync();

            IEnumerable<SelectEmployeesForProjectFormModel> allEmployees = await projectService.AllEmployeesForProjectAsync(project1.Id.ToString());

            var employee = allEmployees.Count();
            Assert.That(employee, Is.EqualTo(2));
        }

        [Test]
        public async Task AllEmployeesForProjectAsyncShouldReturnCorrectDataWhenNoActoveEmployeeInProject()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee1");
            employee1.IsActive = false;
            Guid employee1Id = employee1.Id;

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee2",
                LastName = "TestLastNameEmployee2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee2",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee2");
            employee2.IsActive = false;
            Guid employee2Id = employee2.Id;


            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName1");
            project1.IsActive = true;
            Guid project1Id = project1.Id;

            List<ProjectEmployee> idList = new List<ProjectEmployee>()
            {
                new()
                {
                    EmployeeId = employee1Id,
                    ProjectId = project1Id
                },
                new()
                {
                    EmployeeId = employee2Id,
                    ProjectId = project1Id
                }
            };
            await data.ProjectsEmployees.AddRangeAsync(idList);

            await data.SaveChangesAsync();

            IEnumerable<SelectEmployeesForProjectFormModel> allEmployees = await projectService.AllEmployeesForProjectAsync(project1.Id.ToString());

            var employee = allEmployees.Count();
            Assert.That(employee, Is.EqualTo(0));
        }
        
        [Test]
        public async Task SaveAllEmployeesForProjectAsyncShouldReturnCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee1");
            employee1.IsActive = true;
            Guid employee1Id = employee1.Id;

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee2",
                LastName = "TestLastNameEmployee2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee2",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee2");
            employee2.IsActive = true;
            Guid employee2Id = employee2.Id;

            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName1");
            project1.IsActive = true;
            string project1Id = project1.Id.ToString();

            IEnumerable<string> idList = new List<string>()
            {
                employee1Id.ToString(),
                employee2Id.ToString()
            };

            await data.SaveChangesAsync();

            int allEmployeesInProjectBeforeAction = this.employeeService.AllEmployeesByProjectIdAsync(project1Id).Result.Count();
            await projectService.SaveAllEmployeesForProjectAsync(project1Id, idList);
            int allEmployeesInProjectAfterAction = this.employeeService.AllEmployeesByProjectIdAsync(project1Id).Result.Count();

            Assert.That(allEmployeesInProjectBeforeAction, Is.EqualTo(0));
            Assert.That(allEmployeesInProjectAfterAction, Is.EqualTo(2));
        }

        [Test]
        public async Task GetProjectByIdAsyncShouldReturnCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee1");
            employee1.IsActive = true;
            Guid employee1Id = employee1.Id;

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee2",
                LastName = "TestLastNameEmployee2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee2",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee2");
            employee2.IsActive = true;
            Guid employee2Id = employee2.Id;

            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName1");
            project1.IsActive = true;
            Guid project1Id = project1.Id;

            List<ProjectEmployee> idList = new List<ProjectEmployee>()
            {
                new()
                {
                    EmployeeId = employee1Id,
                    ProjectId = project1Id
                },
                new()
                {
                    EmployeeId = employee2Id,
                    ProjectId = project1Id
                }
            };
            await data.ProjectsEmployees.AddRangeAsync(idList);

            await data.SaveChangesAsync();

            var result =  await projectService.GetProjectByIdAsync(project1Id.ToString());

            Assert.That(result.Name, Is.EqualTo("TestProjectName1"));
            Assert.That(result.Description, Is.EqualTo("TestProjectDescription1"));
            Assert.That(result.ClientId, Is.EqualTo(clientId));
            Assert.That(result.ManagerId, Is.EqualTo(managerId1));
            Assert.That(result.Status, Is.EqualTo((ProjectStatusEnums)1));
            Assert.That(result.ProjectEndDate.ToString("dd.MM.yyyy"), Is.EqualTo("15.12.2023"));
        }

        [Test]
        public async Task GetManagerIdByProjectIdAsyncShouldReturnCorrectManagerId()
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

            //Project info
            Guid managerId = employee.Id;
            Guid clientId = client.Id;
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "2023-12-15",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            await data.SaveChangesAsync();


            string projectId = project.Id.ToString();
            string managerIdExpected = modelProject.ManagerId.ToString();
            string managerIdActual = await projectService.GetManagerIdByProjectIdAsync(projectId);
            Assert.That(managerIdActual, Is.EqualTo(managerIdExpected));
        }

        [Test]
        public async Task IsEmployeeOfProjectAsyncShouldReturnTrueIfEmployeeIsIncludedInProject()
        {
            await using var data = DatabaseMock.MockDatabase();

            var userManager = UserManagerMock.MockUserManager().Object;

            //Client info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
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

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee1 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee1);
            var employee1 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee1");
            employee1.IsActive = false;
            Guid employee1Id = employee1.Id;

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                Email = "TestEmailEmployee11@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee2 = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("3E7C7E56-8A1F-4D12-B9E9-4443FFE5531D"),
                FirstName = "TestFirstNameEmployee2",
                LastName = "TestLastNameEmployee2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressEmployee2",
                JobTitle = "Surveyor"
            };
            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(modelEmployee2);
            var employee2 = data.Employees.FirstOrDefault(x => x.FirstName == "TestFirstNameEmployee2");
            employee2.IsActive = false;
            Guid employee2Id = employee2.Id;


            //Project1 info
            Guid managerId1 = employee1.Id;
            Guid clientId = client.Id;
            var modelProject1 = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = clientId,
                ManagerId = managerId1,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "15.12.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject1);
            var project1 = data.Projects.FirstOrDefault(p => p.Name == "TestProjectName1");
            project1.IsActive = true;
            Guid project1Id = project1.Id;

            List<ProjectEmployee> idList = new List<ProjectEmployee>()
            {
                new()
                {
                    EmployeeId = employee1Id,
                    ProjectId = project1Id
                },
            };
            await data.ProjectsEmployees.AddRangeAsync(idList);

            await data.SaveChangesAsync();

            bool isEmployeeOfProjectShouldBeTrue = await projectService.IsEmployeeOfProjectAsync(project1Id.ToString(), employee1Id.ToString());
            bool isEmployeeOfProjectShouldBeFalse = await projectService.IsEmployeeOfProjectAsync(project1Id.ToString(), employee2Id.ToString());

            Assert.That(isEmployeeOfProjectShouldBeTrue, Is.True);
            Assert.That(isEmployeeOfProjectShouldBeFalse, Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
