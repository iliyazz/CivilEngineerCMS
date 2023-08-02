namespace CivilEngineerCMS.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ExpenseEntityConfiguration : IEntityTypeConfiguration<Expense>
    {
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

            //builder.HasData(GenerateExpenses());
        }

        //private Expense[] GenerateExpenses()
        //{
        //    ICollection<Expense> expenses = new HashSet<Expense>();
        //    Expense expense;
        //    expense = new Expense
        //    {
        //        Id = Guid.Parse("1B445A13-26DA-4943-85E9-4484514B40E5"),
        //        ProjectId = Guid.Parse("EEA8E425-92D8-4BD9-B8A7-638620F070E0"),
        //        Amount = 300.00m,
        //        TotalAmount = 3000.00m,
        //        Date = DateTime.Parse("2023-07-30 13:29:17.7305971"),
        //    };
        //    expenses.Add(expense);
        //    expense = new Expense
        //    {
        //        Id = Guid.Parse("D45C8AAA-9A81-4C51-A8B3-4BB290DC838A"),
        //        ProjectId = Guid.Parse("C54BC710-EFA4-4A2C-8160-812D276D5F3F"),
        //        Amount = 400.00m,
        //        TotalAmount = 800.00m,
        //        Date = DateTime.Parse("2023-07-25 19:08:41.7883026"),
        //    };
        //    expenses.Add(expense);
        //    expense = new Expense
        //    {
        //        Id = Guid.Parse("0F7A556E-2D7B-416A-8F20-8802A73CCC9F"),
        //        ProjectId = Guid.Parse("BF07487F-B5EA-47A7-820C-B5B19353A104"),
        //        Amount = 1500.00m,
        //        TotalAmount = 15000.00m,
        //        Date = DateTime.Parse("2023-07-20 06:15:38.8332714"),
        //    };
        //    expenses.Add(expense);
        //    expense = new Expense
        //    {
        //        Id = Guid.Parse("B3BE497F-A6C9-4716-84CD-CB87D61A473C"),
        //        ProjectId = Guid.Parse("28766CD4-25BA-4B4C-8B68-366197404EE5"),
        //        Amount = 355.00m,
        //        TotalAmount = 3550.00m,
        //        Date = DateTime.Parse("2023-07-25 17:41:15.1157768"),
        //    };
        //    expenses.Add(expense);
        //    expense = new Expense
        //    {
        //        Id = Guid.Parse("5EA52CB2-6071-4226-A230-CE9A7B33583E"),
        //        ProjectId = Guid.Parse("38CF7267-BA70-442B-896F-A41F1A4189BC"),
        //        Amount = 4500.00m,
        //        TotalAmount = 45000.00m,
        //        Date = DateTime.Parse("2023-07-19 20:19:54.8518049"),
        //    };
        //    expenses.Add(expense);
        //    expense = new Expense
        //    {
        //        Id = Guid.Parse("B52C5993-6980-47AD-A6A2-E17CE73A97C0"),
        //        ProjectId = Guid.Parse("D1615808-4CC0-4C32-AD90-9FBE2DE229C3"),
        //        Amount = 1250.00m,
        //        TotalAmount = 12500.00m,
        //        Date = DateTime.Parse("2023-07-25 03:43:03.6582127"),
        //    };
        //    expenses.Add(expense);

        //    return expenses.ToArray();
        //}
    }
}

