namespace CivilEngineerCMS.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CivilEngineerCMS.Common;
    using CivilEngineerCMS.Data;
    using CivilEngineerCMS.Web.ViewModels.Client;
    using CivilEngineerCMS.Web.ViewModels.Employee;
    using CivilEngineerCMS.Web.ViewModels.Expenses;
    using CivilEngineerCMS.Web.ViewModels.Project;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    [TestFixture]
    public class ExpenseTest
    {
        private CivilEngineerCmsDbContext dbContext;
        private ClientService clientService;
        private UserService userService;
        private EmployeeService employeeService;
        private ProjectService projectService;
        private ExpenseService expenseService;
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
            expenseService = new ExpenseService(dbContext);

        }

        [Test]
        public async Task GetExpensesByProjectIdIdAsyncShouldReturnCorrectExpense()
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
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            //Expense info
            var modelExpense = new AddAndEditExpensesFormModel
            {
                ProjectId = project.Id,
                Amount = 380,
                TotalAmount = 3800,
                Date = DateTime.Parse("2023-01-15"),
            };

            await expenseService.CreateExpenseAsync(project.Id.ToString(), modelExpense);



            string projectId = project.Id.ToString();
            AddAndEditExpensesFormModel result = await expenseService.GetExpensesByProjectIdIdAsync(projectId);
            Assert.That(result.Amount, Is.EqualTo(modelExpense.Amount));
        }

        [Test]
        public async Task ExpenseExistsByProjectIdAsyncTrueIfExpenseExists()
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
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            //Expense info
            var modelExpense = new AddAndEditExpensesFormModel
            {
                ProjectId = project.Id,
                Amount = 380,
                TotalAmount = 3800,
                Date = DateTime.Parse("2023-01-15"),
            };

            await expenseService.CreateExpenseAsync(project.Id.ToString(), modelExpense);

            await data.SaveChangesAsync();

            var result = await expenseService.ExpenseExistsByProjectIdAsync(project.Id.ToString());
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ExpenseExistsByProjectIdAsyncFalseIfExpenseDoesNotExist()
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
            var project = await data.Projects.FirstOrDefaultAsync(x => x.Name == "TestProjectName1");
            project.IsActive = true;

            await data.SaveChangesAsync();

            var result = await expenseService.ExpenseExistsByProjectIdAsync(project.Id.ToString());
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CreateExpenseAsyncShouldCreateExpenseCorrectly()
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

            //Expense info
            var modelExpense = new AddAndEditExpensesFormModel
            {
                ProjectId = project.Id,
                Amount = 380,
                TotalAmount = 3800,
                Date = DateTime.Parse("15.01.2023"),
            };

            await expenseService.CreateExpenseAsync(project.Id.ToString(), modelExpense);

            await data.SaveChangesAsync();

            AddAndEditExpensesFormModel result = await expenseService.GetExpensesByProjectIdIdAsync(project.Id.ToString());
            Assert.That(result.Amount, Is.EqualTo(modelExpense.Amount));
            Assert.That(result.TotalAmount, Is.EqualTo(modelExpense.TotalAmount));
        }

        [Test]
        public async Task GetExpenseForEditByProjectIdAsyncShouldReturnExpenseCorrectly()
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

            //Expense info
            var modelExpense = new AddAndEditExpensesFormModel
            {
                ProjectId = project.Id,
                Amount = 380,
                TotalAmount = 3800,
                Date = DateTime.Parse("15.01.2023"),
            };

            await expenseService.CreateExpenseAsync(project.Id.ToString(), modelExpense);

            await data.SaveChangesAsync();

            AddAndEditExpensesFormModel result = await expenseService.GetExpenseForEditByProjectIdAsync(project.Id.ToString());
            Assert.That(result.Amount, Is.EqualTo(modelExpense.Amount));
            Assert.That(result.TotalAmount, Is.EqualTo(modelExpense.TotalAmount));
        }

        [Test]
        public async Task EditExpenseForEditByProjectIdAsyncShouldEditExpenseCorrectly()
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

            //Expense info
            var modelExpense = new AddAndEditExpensesFormModel
            {
                ProjectId = project.Id,
                Amount = 380,
                TotalAmount = 3800,
                Date = DateTime.Parse("15.01.2023"),
            };

            await expenseService.CreateExpenseAsync(project.Id.ToString(), modelExpense);

            var modelExpenseEdited = new AddAndEditExpensesFormModel
            {
                ProjectId = project.Id,
                Amount = 520,
                TotalAmount = 5200,
                Date = DateTime.Parse("15.08.2023"),
            };

            await expenseService.EditExpenseForEditByProjectIdAsync(project.Id.ToString(), modelExpenseEdited);
            AddAndEditExpensesFormModel result = await expenseService.GetExpenseForEditByProjectIdAsync(project.Id.ToString());
            Assert.That(result.Amount, Is.EqualTo(modelExpenseEdited.Amount));
            Assert.That(result.TotalAmount, Is.EqualTo(modelExpenseEdited.TotalAmount));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
