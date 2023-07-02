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
            //employee = new Employee
            //{
            //    Id = Guid.Parse("EC5497AA-1B1C-44AC-8259-7B7B95B07B12"),
            //    FirstName = "Ivan",
            //    LastName = "Ivanov",
            //    PhoneNumber = "+395123456781",
            //    Address = "Sofia",
            //    JobTitle = "Surveyor",
            //    UserId = Guid.Parse("7083ACE7-CBF9-4B28-688C-08DB75A471DC"),
            //};
            //employees.Add(employee);
            //employee = new Employee
            //{
            //    Id = Guid.Parse("68AA7BC7-7358-47E5-985B-BFD6F41BD172"),
            //    JobTitle = "Civil engineer",
            //    UserId = Guid.Parse("6E551113-5037-417E-6890-08DB75A471DC"),
            //};
            //employees.Add(employee);
            //employee = new Employee
            //{
            //    Id = Guid.Parse("674063AF-0A49-4EEF-AC2E-D8F5C82CC827"),
            //    JobTitle = "Water and wastewater engineer",
            //    UserId = Guid.Parse("6556BDC8-DC7D-4E46-6891-08DB75A471DC"),
            //};
            //employees.Add(employee);
            //employee = new Employee
            //{
            //    Id = Guid.Parse("5D3E87B9-D4E2-4308-8EF7-427B410529A5"),
            //    JobTitle = "Structural engineer",
            //    UserId = Guid.Parse("D80D1EF7-3774-4B0C-6892-08DB75A471DC"),
            //};
            return employees.ToArray();
        }
    }
}