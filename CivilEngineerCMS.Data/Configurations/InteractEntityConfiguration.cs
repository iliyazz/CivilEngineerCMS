namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class InteractEntityConfiguration : IEntityTypeConfiguration<Interaction>
    {
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
/*
            builder
                .Property(e => e.Date)
                .HasDefaultValueSql("GETDATE()");
 */
