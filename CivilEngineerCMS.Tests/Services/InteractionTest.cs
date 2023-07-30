namespace CivilEngineerCMS.Tests.Services
{
    using CivilEngineerCMS.Common;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Employee;
    using CivilEngineerCMS.Web.ViewModels.Project;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using Web.ViewModels.Interaction;

    [TestFixture]
    public class InteractionTest
    {
        private CivilEngineerCmsDbContext dbContext;
        private ClientService clientService;
        private UserService userService;
        private EmployeeService employeeService;
        private ProjectService projectService;
        private ExpenseService expenseService;
        private InteractionService interactionService;
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

            //userManager = new UserManager<ApplicationUser>(userManagerMock.Object, null, null, null, null, null, null, null, null);
            userService = new UserService(dbContext, userManager.Object);
            clientService = new ClientService(dbContext, userManager.Object, userService);
            employeeService = new EmployeeService(dbContext, userManager.Object, userService);
            projectService = new ProjectService(dbContext);
            expenseService = new ExpenseService(dbContext);
            interactionService = new InteractionService(dbContext);
        }

        [Test]
        public async Task CreateInteractionAsyncShouldCreateInteractionCorrectly()
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
                ProjectEndDate = "15.01.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            //Interaction info
            var modelInteraction1 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 1",
                Date = DateTime.Parse("15.08.2023"),
                Description = "TestInteractionDescription1",
                Message = "Message1 about design",
                UrlPath = "https://localhost:7255/images/1.jpg",
            };
            var modelInteraction2 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 2",
                Date = DateTime.Parse("16.08.2023"),
                Description = "TestInteractionDescription2",
                Message = "Message2 about design",
                UrlPath = "https://localhost:7255/images/2.jpg",
            };

            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction1);
            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction2);

            IEnumerable<AddAndEditInteractionFormModel> result = await interactionService.AllInteractionsByProjectIdAsync(project.Id.ToString());
            var ineraction1 =  result.FirstOrDefault(x => x.Type == "Phone call 1");
            
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Phone call 1", ineraction1.Type);
            Assert.AreEqual("TestInteractionDescription1", ineraction1.Description);
            Assert.AreEqual("Message1 about design", ineraction1.Message);
            Assert.AreEqual("https://localhost:7255/images/1.jpg", ineraction1.UrlPath);
            Assert.AreEqual(DateTime.Parse("15.08.2023"), ineraction1.Date);
        }

        [Test]
        public async Task InteractionExistsByProjectIdAsyncShouldReturnTrue()
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
                ProjectEndDate = "15.01.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            //Interaction info
            var modelInteraction1 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 1",
                Date = DateTime.Parse("15.08.2023"),
                Description = "TestInteractionDescription1",
                Message = "Message1 about design",
                UrlPath = "https://localhost:7255/images/1.jpg",
            };
            var modelInteraction2 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 2",
                Date = DateTime.Parse("16.08.2023"),
                Description = "TestInteractionDescription2",
                Message = "Message2 about design",
                UrlPath = "https://localhost:7255/images/2.jpg",
            };

            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction1);
            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction2);

            bool result = await interactionService.InteractionExistsByProjectIdAsync(project.Id.ToString());
            Assert.True(result);
        }

        [Test]
        public async Task InteractionExistsByProjectIdAsyncShouldReturnFalseIfProjectDoesNotExist()
        {
            var userManager = UserManagerMock.MockUserManager().Object;

            string projectId = "B1E231F4-7057-45DE-825A-DA8740BB5E6B";

            bool result = await interactionService.InteractionExistsByProjectIdAsync(projectId);
            Assert.False(result);
        }

        [Test]
        public async Task InteractionExistsByProjectIdAsyncShouldReturnFalse()
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
                ProjectEndDate = "15.01.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            bool result = await interactionService.InteractionExistsByProjectIdAsync(project.Id.ToString());
            Assert.False(result);
        }

        [Test]
        public async Task GetInteractionForEditByProjectIdAsyncShouldReturnCorrectData()
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
                ProjectEndDate = "15.01.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            //Interaction info
            var modelInteraction1 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 1",
                Date = DateTime.Parse("15.08.2023"),
                Description = "TestInteractionDescription1",
                Message = "Message1 about design",
                UrlPath = "https://localhost:7255/images/1.jpg",
            };
            var modelInteraction2 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 2",
                Date = DateTime.Parse("16.08.2023"),
                Description = "TestInteractionDescription2",
                Message = "Message2 about design",
                UrlPath = "https://localhost:7255/images/2.jpg",
            };

            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction1);
            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction2);
            var result = await this.dbContext.Interactions.FirstOrDefaultAsync(x => x.Type == "Phone call 1");
            var interaction1Id = result.Id.ToString();

            var interaction1 = await interactionService.GetInteractionForEditByProjectIdAsync(project.Id.ToString(), interaction1Id);

            Assert.That(interaction1.Date, Is.EqualTo(modelInteraction1.Date));
            Assert.That(interaction1.Description, Is.EqualTo(modelInteraction1.Description));
            Assert.That(interaction1.Message, Is.EqualTo(modelInteraction1.Message));
            Assert.That(interaction1.UrlPath, Is.EqualTo(modelInteraction1.UrlPath));
            Assert.That(interaction1.Type, Is.EqualTo(modelInteraction1.Type));
        }

        [Test]
        public async Task EditInteractionByProjectIdAsyncShouldReturnCorrectData()
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
                ProjectEndDate = "15.01.2023",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            //Interaction info
            var modelInteraction1 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 1",
                Date = DateTime.Parse("15.08.2023"),
                Description = "TestInteractionDescription1",
                Message = "Message1 about design",
                UrlPath = "https://localhost:7255/images/1.jpg",
            };
            var modelInteraction2 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 2",
                Date = DateTime.Parse("16.08.2023"),
                Description = "TestInteractionDescription2",
                Message = "Message2 about design",
                UrlPath = "https://localhost:7255/images/2.jpg",
            };

            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction1);
            await interactionService.CreateInteractionAsync(project.Id.ToString(), modelInteraction2);

            var modelInteraction3 = new AddAndEditInteractionFormModel
            {
                ProjectId = project.Id,
                Type = "Phone call 3",
                Date = DateTime.Parse("15.09.2023"),
                Description = "TestInteractionDescription3",
                Message = "Message3 about design",
                UrlPath = "https://localhost:7255/images/3.jpg",
            };

            var result1 = await this.dbContext.Interactions.FirstOrDefaultAsync(x => x.Type == "Phone call 1");
            var interaction1Id = result1.Id.ToString();
            await interactionService.EditInteractionByProjectIdAsync(interaction1Id, modelInteraction3);
            var result3 = await this.dbContext.Interactions.FirstOrDefaultAsync(x => x.Type == "Phone call 3");

            Assert.That(result3.Description, Is.EqualTo(modelInteraction3.Description));
            Assert.That(result3.Message, Is.EqualTo(modelInteraction3.Message));
            Assert.That(result3.UrlPath, Is.EqualTo(modelInteraction3.UrlPath));
            Assert.That(result3.Type, Is.EqualTo(modelInteraction3.Type));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
