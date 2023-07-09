namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(GenerateEmployees());
        }

        private Employee[] GenerateEmployees()
        {
            ICollection<Employee> employees = new HashSet<Employee>();
            Employee employee;
            employee = new Employee
            {
                Id = Guid.Parse("8C0629B6-B564-4A2C-A6C2-73408D4878E5"),
                FirstName = "Ivan",
                LastName = "Ivanov",
                PhoneNumber = "+395123456781",
                Address = "Sofia",
                JobTitle = "Surveyor",
                UserId = Guid.Parse("5544BB21-BE62-44AC-1D76-08DB7A21C396"),

            };
            employees.Add(employee);

            return employees.ToArray();
        }
    }
}