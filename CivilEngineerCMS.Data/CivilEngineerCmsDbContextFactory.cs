namespace CivilEngineerCMS.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class CivilEngineerCmsDbContextFactory : IDesignTimeDbContextFactory<CivilEngineerCmsDbContext>
    {
        public CivilEngineerCmsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CivilEngineerCmsDbContext>();
            optionsBuilder.UseSqlite("Data Source=civilengineercms.db");

            return new CivilEngineerCmsDbContext(optionsBuilder.Options);
        }
    }
}
