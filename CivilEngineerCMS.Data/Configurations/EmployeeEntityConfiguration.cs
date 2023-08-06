namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        /// <summary>
        /// This method configure Employee entity and is called by cref="DbContext.OnModelCreating(ModelBuilder)"/>
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .Property(e => e.IsActive)
                .HasDefaultValue(true);

        }
    }
}