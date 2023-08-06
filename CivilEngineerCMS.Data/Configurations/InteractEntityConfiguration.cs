namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class InteractEntityConfiguration : IEntityTypeConfiguration<Interaction>
    {
        /// <summary>
        /// This method configure Interaction entity and is called by cref="DbContext.OnModelCreating(ModelBuilder)"/>
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Interaction> builder)
        {
            builder
                .Property(i => i.Date)
                .HasDefaultValueSql("GETDATE()");
            builder
                .HasOne(i => i.Project)
                .WithMany(p => p.Interactions)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

