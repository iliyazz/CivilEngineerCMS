namespace CivilEngineerCMS.Data.Configurations
{
    using Common;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .Property(p => p.ProjectCreatedDate)
                .HasDefaultValueSql("GETDATE()");
            builder
                .HasOne(p => p.Client)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.Manager)
                .WithMany(e => e.Projects)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);


            //builder.HasData(this.GenerateProjects());
        }
        
    }
}