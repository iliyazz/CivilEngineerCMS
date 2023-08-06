namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ExpenseEntityConfiguration : IEntityTypeConfiguration<Expense>
    {
        /// <summary>
        /// This method configure Expense entity and is called by cref="DbContext.OnModelCreating(ModelBuilder)"/>
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder
                .Property(e => e.Date)
                .HasDefaultValueSql("GETDATE()");
            builder
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");
            builder
                .Property(e => e.TotalAmount)
                .HasColumnType("decimal(18,2)");
        }
    }
}

