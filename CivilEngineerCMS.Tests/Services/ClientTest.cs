namespace CivilEngineerCMS.Tests.Services
{
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Project;

    using Common;

    using Data;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using Web.ViewModels.Employee;

    [TestFixture]
    public class ClientTest
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

            //userManager = new UserManager<ApplicationUser>(userManagerMock.Object, null, null, null, null, null, null, null, null);
            userService = new UserService(dbContext, userManager.Object);
            clientService = new ClientService(dbContext, userManager.Object, userService);
            employeeService = new EmployeeService(dbContext, userManager.Object, userService);
            projectService = new ProjectService(dbContext);

        }


        [Test]
        public async Task CreateClientAsyncShouldAddClientToDatabase()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };
            //await data.SaveChangesAsync();
             


            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            Assert.That(data.Clients.Count(), Is.EqualTo(1));
        }


        [Test]
        public async Task CreateClientAsyncShouldAddClientToDatabaseWithCorrectData()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };
            await data.SaveChangesAsync();
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            Assert.That(client.FirstName, Is.EqualTo("TestFirstName1"));
            Assert.That(client.LastName, Is.EqualTo("TestLastName1"));
            Assert.That(client.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(client.Address, Is.EqualTo("TestAddress1"));
        }

        [Test]
        public async Task ClientExistsByIdAsyncShouldReturnTrueIfClientExists()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            await data.SaveChangesAsync();
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientId = client.Id.ToString();
            var result = await clientService.ClientExistsByIdAsync(clientId);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ClientExistsByIdAsyncShouldReturnFalseIfClientDoesNotExist()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            await data.SaveChangesAsync();
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = false;
            await data.SaveChangesAsync();
            string clientId = client.Id.ToString();
            var result = await clientService.ClientExistsByIdAsync(clientId);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DetailsClientAsyncShouldReturnCorrectDataOfClient()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmail1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
            await data.Users.AddAsync(applicationUser);

            var model = new CreateClientFormModel
                {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientId = client.Id.ToString();
            var result = await clientService.DetailsClientAsync(clientId);
            Assert.That(result.FirstName, Is.EqualTo("TestFirstName1"));
            Assert.That(result.LastName, Is.EqualTo("TestLastName1"));
            Assert.That(result.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(result.Address, Is.EqualTo("TestAddress1"));
            Assert.That(result.Email, Is.EqualTo("TestEmail1@mail.com"));
        }

        [Test]
        public async Task GetClientForEditByIdAsyncShouldReturnCorrectData()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmail1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
            await data.Users.AddAsync(applicationUser);

            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientId = client.Id.ToString();
            var result = await clientService.GetClientForEditByIdAsync(clientId);
            Assert.That(result.FirstName, Is.EqualTo("TestFirstName1"));
            Assert.That(result.LastName, Is.EqualTo("TestLastName1"));
            Assert.That(result.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(result.Address, Is.EqualTo("TestAddress1"));
            Assert.That(result.Email, Is.EqualTo("TestEmail1@mail.com"));
        }

        [Test]
        public async Task EditClientByIdAsyncShouldEditClientCorrectly()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmail1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
            await data.Users.AddAsync(applicationUser);

            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientId = client.Id.ToString();
            var editModel = new EditClientFormModel
            {
                FirstName = "TestFirstName2",
                LastName = "TestLastName2",
                PhoneNumber = "+359123456789",
                Address = "TestAddress2",
            };
            await clientService.EditClientByIdAsync(clientId, editModel);
            var result = await clientService.DetailsClientAsync(clientId);
            Assert.That(result.FirstName, Is.EqualTo("TestFirstName2"));
            Assert.That(result.LastName, Is.EqualTo("TestLastName2"));
            Assert.That(result.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(result.Address, Is.EqualTo("TestAddress2"));
        }

        [Test]
        public async Task GetClientForPreDeleteByIdAsyncShouldReturnCorrectData()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmail1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
            await data.Users.AddAsync(applicationUser);

            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientId = client.Id.ToString();
            var deleteModel = await clientService.GetClientForPreDeleteByIdAsync(clientId);
            Assert.That(deleteModel.FirstName, Is.EqualTo("TestFirstName1"));
            Assert.That(deleteModel.LastName, Is.EqualTo("TestLastName1"));
            Assert.That(deleteModel.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(deleteModel.Address, Is.EqualTo("TestAddress1"));
            Assert.That(deleteModel.Email, Is.EqualTo("TestEmail1@mail.com"));
        }

        [Test]
        public async Task DeleteClientByIdAsyncShouldDeleteClientCorrectly()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmail1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
            await data.Users.AddAsync(applicationUser);

            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientId = client.Id.ToString();
            var countOfClientBeforeDelete = data.Clients.Count(c => c.IsActive);
            await clientService.DeleteClientByIdAsync(clientId);
            var countOfClientAfterDelete = data.Clients.Count(c => c.IsActive);
            Assert.That(countOfClientBeforeDelete, Is.EqualTo(1));
            Assert.That(countOfClientAfterDelete, Is.EqualTo(0));

        }


        [Test]
        public async Task GetClientIdByProjectIdAsyncShouldReturnCorrectClientId()
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
            string clientIdActual = await clientService.GetClientIdByProjectIdAsync(projectId);
            Assert.That(clientIdActual, Is.EqualTo(clientIdExpected));
        }

        [Test]
        public async Task GetClientIdByProjectIdAsyncShouldReturnNullIfProjectNotExist()
        {
            await using var data = DatabaseMock.MockDatabase();
            var result = await clientService.GetClientIdByProjectIdAsync("1FA37C21-89E7-4F63-853A-43BC8B5B6504");
            Assert.That(result, Is.Null);
        }


        [Test]
        public async Task IsClientByUserIdAsyncShouldReturnTrueIfClientExists()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            //await data.SaveChangesAsync();
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientUserId = client.UserId.ToString();
            var result = await clientService.IsClientByUserIdAsync(clientUserId);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsClientByUserIdAsyncShouldReturnFalseIfClientDoesNotExist()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            //await data.SaveChangesAsync();
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientUserId = Guid.NewGuid().ToString();
            var result = await clientService.IsClientByUserIdAsync(clientUserId);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetClientIdByUserIdAsyncShouldReturnCorrectClientId()
        {
            await using var data = DatabaseMock.MockDatabase();
            //await data.SaveChangesAsync();

            var userManager = UserManagerMock.MockUserManager().Object;
            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                PhoneNumber = "+359123456789",
                Address = "TestAddress1",
            };

            //await data.SaveChangesAsync();
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client = data.Clients.FirstOrDefault();
            client.IsActive = true;
            await data.SaveChangesAsync();
            string clientUserId = client.UserId.ToString();
            string clientIdExpected = client.Id.ToString();
            string clientIdActual = await clientService.GetClientIdByUserIdAsync(clientUserId);
            Assert.That(clientIdActual, Is.EqualTo(clientIdExpected));
        }

        [Test]
        public async Task AllClientsForViewAsyncShouldReturnCorrectClients()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);



            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstNameClient1",
                LastName = "TestLastNameClient1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressClient1",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client1 = data.Clients.FirstOrDefault(x => x.UserId == model.UserId);
            client1.IsActive = true;
            await data.SaveChangesAsync();


            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                Email = "TestEmailClient1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            model = new CreateClientFormModel
            {
                UserId = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                FirstName = "TestFirstNameClient2",
                LastName = "TestLastNameClient2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressClient2",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client2 = data.Clients.FirstOrDefault(x => x.UserId == model.UserId);
            client2.IsActive = true;

            await data.SaveChangesAsync();

            IEnumerable<AllClientViewModel> result = await clientService.AllClientsForViewAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().FirstName, Is.EqualTo(client1.FirstName));
            Assert.That(result.Last().FirstName, Is.EqualTo(client2.FirstName));
            Assert.That(result.First().LastName, Is.EqualTo(client1.LastName));
            Assert.That(result.Last().LastName, Is.EqualTo(client2.LastName));
            Assert.That(result.First().PhoneNumber, Is.EqualTo(client1.PhoneNumber));
            Assert.That(result.Last().PhoneNumber, Is.EqualTo(client2.PhoneNumber));
            Assert.That(result.First().Address, Is.EqualTo(client1.Address));
            Assert.That(result.Last().Address, Is.EqualTo(client2.Address));
            Assert.That(result.First().Email, Is.EqualTo(client1.User.Email));
            Assert.That(result.Last().Email, Is.EqualTo(client2.User.Email));
        }

        [Test]
        public async Task AllClientsAsyncShouldReturnCorrectClients()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);



            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstNameClient1",
                LastName = "TestLastNameClient1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressClient1",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client1 = data.Clients.FirstOrDefault(x => x.UserId == model.UserId);
            client1.IsActive = true;
            await data.SaveChangesAsync();


            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                Email = "TestEmailClient1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            model = new CreateClientFormModel
            {
                UserId = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                FirstName = "TestFirstNameClient2",
                LastName = "TestLastNameClient2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressClient2",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client2 = data.Clients.FirstOrDefault(x => x.UserId == model.UserId);
            client2.IsActive = true;

            await data.SaveChangesAsync();

            IEnumerable<SelectClientForProjectFormModel> result = await clientService.AllClientsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().FirstName, Is.EqualTo(client1.FirstName));
            Assert.That(result.Last().FirstName, Is.EqualTo(client2.FirstName));
            Assert.That(result.First().LastName, Is.EqualTo(client1.LastName));
            Assert.That(result.Last().LastName, Is.EqualTo(client2.LastName));
            Assert.That(result.First().PhoneNumber, Is.EqualTo(client1.PhoneNumber));
            Assert.That(result.Last().PhoneNumber, Is.EqualTo(client2.PhoneNumber));
            Assert.That(result.First().Address, Is.EqualTo(client1.Address));
            Assert.That(result.Last().Address, Is.EqualTo(client2.Address));
            Assert.That(result.First().Email, Is.EqualTo(client1.User.Email));
            Assert.That(result.Last().Email, Is.EqualTo(client2.User.Email));
        }

        [Test]
        public async Task AllClientsAsyncShouldReturnEmptyCollectionIfNoClients()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            clientService = new ClientService(data, userManager, userService);

            IEnumerable<SelectClientForProjectFormModel> result = await clientService.AllClientsAsync();

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task AllClientsAsyncShouldReturnEmptyCollectionIfNoActiveClients()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);



            var model = new CreateClientFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstNameClient1",
                LastName = "TestLastNameClient1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressClient1",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client1 = data.Clients.FirstOrDefault(x => x.UserId == model.UserId);
            client1.IsActive = false;
            await data.SaveChangesAsync();


            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                Email = "TestEmailClient1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            model = new CreateClientFormModel
            {
                UserId = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                FirstName = "TestFirstNameClient2",
                LastName = "TestLastNameClient2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressClient2",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(model);
            var client2 = data.Clients.FirstOrDefault(x => x.UserId == model.UserId);
            client2.IsActive = false;

            await data.SaveChangesAsync();

            IEnumerable<SelectClientForProjectFormModel> result = await clientService.AllClientsAsync();

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task AllProjectsByUserIdAsyncShouldReturnCorrectProjects()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            //Client1 info
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
            await data.SaveChangesAsync();

            var client1 =  data.Clients.FirstOrDefault(x => x.UserId == modelClient.UserId);
            client1.IsActive = true;

            //Client2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                Email = "TestEmailClien2@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            modelClient = new CreateClientFormModel
            {
                UserId = Guid.Parse("1FA37C21-89E7-4F63-853A-43BC8B5B6504"),
                FirstName = "TestFirstNameClient2",
                LastName = "TestLastNameClient2",
                PhoneNumber = "+359123456788",
                Address = "TestAddressClient2",
            };
            clientService = new ClientService(data, userManager, userService);
            await clientService.CreateClientAsync(modelClient);
            await data.SaveChangesAsync();

            var client2 = data.Clients.FirstOrDefault(x => x.UserId == modelClient.UserId);
            client2.IsActive = true;

            //Employee info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("79485427-F039-402E-8668-2F17A37AACDA"),
                Email = "TestEmailEmployee1@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            var modelEmployee = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("79485427-F039-402E-8668-2F17A37AACDA"),
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
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = client1.Id,
                ManagerId = employee.Id,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "2023-12-15",
                EmployeeId = employee.Id,
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project = data.Projects.FirstOrDefault();
            project.IsActive = true;
            project.ProjectCreatedDate = DateTime.Parse("2023-12-15");

            await data.SaveChangesAsync();

            var client1Projects = await clientService.AllProjectsByUserIdAsync(client1.UserId.ToString());
            var client2Projects = await clientService.AllProjectsByUserIdAsync(client2.UserId.ToString());

            Assert.That(client1Projects.Count(), Is.EqualTo(1));
            Assert.That(client2Projects.Count(), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
