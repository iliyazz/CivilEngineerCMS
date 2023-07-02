﻿namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ExpenseEntityConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");
            builder
                .Property(e => e.TotalAmount)
                .HasColumnType("decimal(18,2)");

        }
    }
}
