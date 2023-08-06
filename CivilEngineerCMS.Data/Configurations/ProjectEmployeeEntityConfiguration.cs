namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ProjectEmployeeEntityConfiguration : IEntityTypeConfiguration<ProjectEmployee>
    {
        /// <summary>
        /// This method configure ProjectEmployee entity and is called by cref="DbContext.OnModelCreating(ModelBuilder)"/>
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
        {
            builder
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });
            builder
                .HasOne(p => p.Employee)
                .WithMany(e => e.ProjectsEmployees)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.Project)
                .WithMany(p => p.ProjectsEmployees)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
