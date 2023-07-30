namespace CivilEngineerCMS.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CivilEngineerCMS.Common;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Employee;
    using CivilEngineerCMS.Web.ViewModels.Manager;
    using CivilEngineerCMS.Web.ViewModels.Project;

    using Microsoft.AspNetCore.Identity;
    using Moq;

    [TestFixture]
    public class EmployeeTest
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
        public async Task EmployeeExistsByIdAsyncShouldReturnTrueIfEmployeeExists()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

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
            await data.SaveChangesAsync();

            var employee1 = data.Employees.FirstOrDefault(x => x.UserId == modelEmployee.UserId);
            var employee1Id = employee1.Id.ToString();
            employee1.IsActive = true;
            
            await data.SaveChangesAsync();

            var result = await employeeService.EmployeeExistsByIdAsync(employee1Id);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task EmployeeExistsByIdAsyncShouldReturnFalseIfEmployeeIsDeleteted()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

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
            await data.SaveChangesAsync();

            var employee1 = data.Employees.FirstOrDefault(x => x.UserId == modelEmployee.UserId);
            var employee1Id = employee1.Id.ToString();
            employee1.IsActive = false;

            await data.SaveChangesAsync();

            var result = await employeeService.EmployeeExistsByIdAsync(employee1Id);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AllEmployeesAsyncShouldReturnCorrectEmployees()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailEmnployee1@mail.com",
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

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                Email = "TestEmailEmnployee2@mail.com",
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


            IEnumerable<AllEmployeeViewModel> result = await employeeService.AllEmployeesAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().FirstName, Is.EqualTo(employee1.FirstName));
            Assert.That(result.Last().FirstName, Is.EqualTo(employee2.FirstName));
            Assert.That(result.First().LastName, Is.EqualTo(employee1.LastName));
            Assert.That(result.Last().LastName, Is.EqualTo(employee2.LastName));
            Assert.That(result.First().PhoneNumber, Is.EqualTo(employee1.PhoneNumber));
            Assert.That(result.Last().PhoneNumber, Is.EqualTo(employee2.PhoneNumber));
            Assert.That(result.First().Email, Is.EqualTo(employee1.User.Email));
            Assert.That(result.Last().Email, Is.EqualTo(employee2.User.Email));
        }

        [Test]
        public async Task AllProjectsByManagerIdAsyncShouldReturnCorrectProjects()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
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
            var employee1Id = employee1.Id.ToString();
            await data.SaveChangesAsync();

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                Email = "TestEmailClient1@mail.com",
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
            var employee2Id = employee2.Id.ToString();
            await data.SaveChangesAsync();

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

            //Client2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("112866A9-FA81-4842-B068-AD3DA9B06FB7"),
                Email = "TestEmailClien2@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            modelClient = new CreateClientFormModel
            {
                UserId = Guid.Parse("112866A9-FA81-4842-B068-AD3DA9B06FB7"),
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

            //Project1 info
            var modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName1",
                Description = "TestProjectDescription1",
                ClientId = client1.Id,
                ManagerId = Guid.Parse(employee1Id),
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "2023-12-15",
                EmployeeId = Guid.Parse(employee1Id),
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project1 = data.Projects.FirstOrDefault(x => x.Name == "TestProjectName1");
            project1.IsActive = true;
            project1.ProjectCreatedDate = DateTime.Parse("2023-12-15");
            await data.SaveChangesAsync();

            //Project2 info
            modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName2",
                Description = "TestProjectDescription2",
                ClientId = client2.Id,
                ManagerId = Guid.Parse(employee1Id),
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "2024-12-15",
                EmployeeId = Guid.Parse(employee2Id)
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project2 = data.Projects.FirstOrDefault(x => x.Name == "TestProjectName2");
            project2.IsActive = true;
            project2.ProjectCreatedDate = DateTime.Parse("2023-12-15");

            await data.SaveChangesAsync();

            var projectsByManager1 = await employeeService.AllProjectsByManagerIdAsync(employee1Id);
            var projectsByManager2 = await employeeService.AllProjectsByManagerIdAsync(employee2Id);
            Assert.That(projectsByManager1.Count(), Is.EqualTo(2));
            Assert.That(projectsByManager2.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task AllEmployeesAndManagersAsyncShouldReturnCorrectEmployeesAndManagers()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
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
            var employee1 = data.Employees.FirstOrDefault();
            //var employee1Id = employee1.Id.ToString();
            employee1.IsActive = true;
            //await data.SaveChangesAsync();

            //Employee2 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                Email = "TestEmailClient2@mail.com",
            };
            await data.Users.AddAsync(applicationUser);

            model = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("25A86FA8-C59F-42E2-B5E6-DC24ABEC7DA0"),
                FirstName = "TestFirstNameEmployee2",
                LastName = "TestLastNameEmployee2",
                PhoneNumber = "+359123456789",
                Address = "TestAddressEmployee2",
                JobTitle = "Surveyor2"
            };
            await employeeService.CreateEmployeeAsync(model);
            var employee2 = data.Employees.LastOrDefault();
            //var employee2Id = employee2.Id.ToString();
            employee2.IsActive = true;
            await data.SaveChangesAsync();

            IEnumerable<SelectEmployeesAndManagerForProjectFormModel> result = await employeeService.AllEmployeesAndManagersAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().FirstName, Is.EqualTo(employee1.FirstName));
            Assert.That(result.Last().FirstName, Is.EqualTo(employee2.FirstName));
            Assert.That(result.First().LastName, Is.EqualTo(employee1.LastName));
            Assert.That(result.Last().LastName, Is.EqualTo(employee2.LastName));
            Assert.That(result.First().JobTitle, Is.EqualTo(employee1.JobTitle));
            Assert.That(result.Last().JobTitle, Is.EqualTo(employee2.JobTitle));
        }

        [Test]
        public async Task AllEmployeesAndManagersAsyncShouldReturnEmptyCollectionIfNoEmployeesAndManagers()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            IEnumerable<SelectEmployeesAndManagerForProjectFormModel> result = await employeeService.AllEmployeesAndManagersAsync();

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetManagerIdByUserIdAsyncShouldReturnCorrectClientId()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
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
            var employee1UserId = model.UserId.ToString();
            var employee1 = data.Employees.FirstOrDefault();
            employee1.IsActive = true;
            await data.SaveChangesAsync();
            string employee1IdExpected = employee1.Id.ToString();
            string employee1IdActual = await employeeService.GetManagerIdByUserIdAsync(employee1UserId);
            Assert.That(employee1IdActual, Is.EqualTo(employee1IdExpected));
        }

        [Test]
        public async Task CreateEmployeeAsyncShouldAddEmployeeToDatabase()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            var model = new CreateEmployeeFormModel
            {
                UserId = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor1"
            };
            await data.SaveChangesAsync();

            employeeService = new EmployeeService(data, userManager, userService);
            await employeeService.CreateEmployeeAsync(model);
            Assert.That(data.Employees.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task DetailsEmployeeAsyncShouldReturnCorrectDataOfEmployee()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmail1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
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
            var employee = data.Employees.FirstOrDefault();
            employee.IsActive = true;
            await data.SaveChangesAsync();
            string employeeId = employee.Id.ToString();
            var result = await employeeService.DetailsEmployeeAsync(employeeId);
            Assert.That(result.FirstName, Is.EqualTo("TestFirstNameEmployee1"));
            Assert.That(result.LastName, Is.EqualTo("TestLastNameEmployee1"));
            Assert.That(result.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(result.Address, Is.EqualTo("TestAddressEmployee1"));
            Assert.That(result.Email, Is.EqualTo("TestEmail1@mail.com"));
        }

        [Test]
        public async Task GetEmployeeForEditByIdAsyncShouldReturnCorrectData()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailEmp[loyee1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
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
            var employee = data.Employees.FirstOrDefault();
            employee.IsActive = true;
            await data.SaveChangesAsync();
            string employeeId = employee.Id.ToString();
            var result = await employeeService.GetEmployeeForEditByIdAsync(employeeId);
            Assert.That(result.FirstName, Is.EqualTo("TestFirstNameEmployee1"));
            Assert.That(result.LastName, Is.EqualTo("TestLastNameEmployee1"));
            Assert.That(result.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(result.Address, Is.EqualTo("TestAddressEmployee1"));
            Assert.That(result.Email, Is.EqualTo("TestEmailEmp[loyee1@mail.com"));
            Assert.That(result.JobTitle, Is.EqualTo("Surveyor1"));
        }

        [Test]
        public async Task EditEmployeeByIdAsyncShouldEditEmployeeCorrectly()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailEmployee1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
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
            var employee = data.Employees.FirstOrDefault();
            employee.IsActive = true;
            await data.SaveChangesAsync();
            string employeeId = employee.Id.ToString();
            var editModel = new EditEmployeeFormModel
            {
                FirstName = "TestFirstNameEmployee1",
                LastName = "TestLastNameEmployee1",
                PhoneNumber = "+359123456789",
                Address = "TestAddressEmployee1",
                JobTitle = "Surveyor1"
            };
            await employeeService.EditEmployeeByIdAsync(employeeId, editModel);
            var result = await employeeService.DetailsEmployeeAsync(employeeId);
            Assert.That(result.FirstName, Is.EqualTo("TestFirstNameEmployee1"));
            Assert.That(result.LastName, Is.EqualTo("TestLastNameEmployee1"));
            Assert.That(result.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(result.Address, Is.EqualTo("TestAddressEmployee1"));
            Assert.That(result.JobTitle, Is.EqualTo("Surveyor1"));
        }

        [Test]
        public async Task GetEmployeeForPreDeleteByIdAsyncShouldReturnCorrectData()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailEmployee1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
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
            var employee = data.Employees.FirstOrDefault();
            employee.IsActive = true;
            await data.SaveChangesAsync();
            string employeeId = employee.Id.ToString();
            var deleteModel = await employeeService.GetEmployeeForPreDeleteByIdAsync(employeeId);

            Assert.That(deleteModel.FirstName, Is.EqualTo("TestFirstNameEmployee1"));
            Assert.That(deleteModel.LastName, Is.EqualTo("TestLastNameEmployee1"));
            Assert.That(deleteModel.PhoneNumber, Is.EqualTo("+359123456789"));
            Assert.That(deleteModel.Address, Is.EqualTo("TestAddressEmployee1"));
            Assert.That(deleteModel.Email, Is.EqualTo("TestEmailEmployee1@mail.com"));
            Assert.That(deleteModel.JobTitle, Is.EqualTo("Surveyor1"));
        }

        [Test]
        public async Task DeleteEmployeeByIdAsyncShouldDeleteEmployeeCorrectly()
        {
            var userManager = UserManagerMock.MockUserManager().Object;
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailEmployee1@mail.com",
            };
            await using var data = DatabaseMock.MockDatabase();
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
            var employee = data.Employees.FirstOrDefault();
            employee.IsActive = true;
            await data.SaveChangesAsync();
            string employeeId = employee.Id.ToString();
            var countOfEmployeesBeforeDelete = data.Employees.Count(c => c.IsActive);
            await employeeService.DeleteEmployeeByIdAsync(employeeId);
            var countOfEmployeesAfterDelete = data.Clients.Count(c => c.IsActive);
            Assert.That(countOfEmployeesBeforeDelete, Is.EqualTo(1));
            Assert.That(countOfEmployeesAfterDelete, Is.EqualTo(0));
        }

        [Test]
        public async Task AllEmployeesByProjectIdAsyncShouldReturnCorrectEmployees()
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

            IEnumerable<AllEmployeeViewModel> result = await employeeService.AllEmployeesByProjectIdAsync(project1Id.ToString());

            var employee1Actual = result.FirstOrDefault(x => x.Id == employee1Id);
            var employee2Actual = result.FirstOrDefault(x => x.Id == employee2Id);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(employee1Actual.FirstName, Is.EqualTo(employee1.FirstName));
            Assert.That(employee2Actual.FirstName, Is.EqualTo(employee2.FirstName));
            Assert.That(employee1Actual.LastName, Is.EqualTo(employee1.LastName));
            Assert.That(employee2Actual.LastName, Is.EqualTo(employee2.LastName));
            Assert.That(employee1Actual.PhoneNumber, Is.EqualTo(employee1.PhoneNumber));
            Assert.That(employee2Actual.PhoneNumber, Is.EqualTo(employee2.PhoneNumber));
            Assert.That(employee1Actual.Email, Is.EqualTo(employee1.User.Email));
            Assert.That(employee2Actual.Email, Is.EqualTo(employee2.User.Email));
            Assert.That(employee1Actual.JobTitle, Is.EqualTo(employee1.JobTitle));
            Assert.That(employee2Actual.JobTitle, Is.EqualTo(employee2.JobTitle));
        }

        [Test]
        public async Task IsEmployeeInProjectAsyncShouldReturnCorrectData()
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
            };
            await data.ProjectsEmployees.AddRangeAsync(idList);
            await data.SaveChangesAsync();

            bool result1 = await employeeService.IsEmployeeInProjectAsync(project1Id.ToString(), employee1Id.ToString());
            bool result2 = await employeeService.IsEmployeeInProjectAsync(project1Id.ToString(), employee2Id.ToString());

            Assert.That(result1, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
        }

        [Test]
        public async Task IsEmployeeAsyncShouldReturnTrueIfUserIsEmployee()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
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
            await data.SaveChangesAsync();

            var employee1 = data.Employees.FirstOrDefault(x => x.UserId == modelEmployee.UserId);
            employee1.IsActive = true;

            await data.SaveChangesAsync();

            var result = await employeeService.IsEmployeeAsync(modelEmployee.UserId.ToString());
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsEmployeeAsyncShouldReturnFalseIfUserIsDeleted()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;
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
            await data.SaveChangesAsync();

            var employee1 = data.Employees.FirstOrDefault(x => x.UserId == modelEmployee.UserId);
            employee1.IsActive = false;

            await data.SaveChangesAsync();

            var result = await employeeService.IsEmployeeAsync(modelEmployee.UserId.ToString());
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetEmployeeIdByUserIdAsyncShouldReturnCorrectClientId()
        {
            await using var data = DatabaseMock.MockDatabase();
            var userManager = UserManagerMock.MockUserManager().Object;

            //Employee1 info
            this.applicationUser = new ApplicationUser()
            {
                Id = Guid.Parse("B1E231F4-7057-45DE-825A-DA8740BB5E6B"),
                Email = "TestEmailClient1@mail.com",
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
            var employee1UserId = model.UserId.ToString();
            var employee1 = data.Employees.FirstOrDefault();
            employee1.IsActive = true;
            await data.SaveChangesAsync();
            string employee1IdExpected = employee1.Id.ToString();
            string employee1IdActual = await employeeService.GetEmployeeIdByUserIdAsync(employee1UserId);
            Assert.That(employee1IdActual, Is.EqualTo(employee1IdExpected));
        }

        [Test]
        public async Task AllProjectsByEmployeeIdAsyncShouldReturnCorrectProjects()
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

            //Project2 info
            modelProject = new AddAndEditProjectFormModel
            {
                Name = "TestProjectName2",
                Description = "TestProjectDescription2",
                ClientId = client1.Id,
                ManagerId = employee1Id,
                Status = (ProjectStatusEnums)1,
                ProjectEndDate = "2024-12-15",
            };
            projectService = new ProjectService(data);
            await projectService.CreateProjectAsync(modelProject);
            var project2 = data.Projects.FirstOrDefault(x => x.Name == "TestProjectName2");
            project2.IsActive = true;
            project2.ProjectCreatedDate = DateTime.Parse("2023-12-15");
            var project2Id = project2.Id;

            await data.SaveChangesAsync();

            List<ProjectEmployee> idList = new List<ProjectEmployee>()
            {
                new()
                {
                    EmployeeId = employee1Id,
                    ProjectId = project1Id
                },
                new()
                {
                    EmployeeId = employee1Id,
                    ProjectId = project2Id
                }
            };
            await data.ProjectsEmployees.AddRangeAsync(idList);
            await data.SaveChangesAsync();

            var projectsByEmployee1 = await employeeService.AllProjectsByEmployeeIdAsync(employee1Id.ToString());
            var projectsByEmployee2 = await employeeService.AllProjectsByEmployeeIdAsync(employee2Id.ToString());
            Assert.That(projectsByEmployee1.Count(), Is.EqualTo(2));
            Assert.That(projectsByEmployee2.Count(), Is.EqualTo(0));
        }



        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
