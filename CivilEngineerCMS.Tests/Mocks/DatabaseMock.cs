namespace CivilEngineerCMS.Tests.Mocks
{
    using Data;

    public static class DatabaseMock
    {

        public static CivilEngineerCmsDbContext MockDatabase()
        {
            DbContextOptionsBuilder<CivilEngineerCmsDbContext> optionsBuilder
                = new DbContextOptionsBuilder<CivilEngineerCmsDbContext>();

            optionsBuilder.UseInMemoryDatabase($"CivilEngineerCMS-{DateTime.Now.Ticks}");

            return new CivilEngineerCmsDbContext(optionsBuilder.Options);
        }
    }
}