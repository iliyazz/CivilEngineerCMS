namespace CivilEngineerCMS.Tests.Mocks;

using Data;

public static class DatabaseMock
{
    //public static CivilEngineerCmsDbContext Instance
    //{
    //    get
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<CivilEngineerCmsDbContext>()
    //            .UseInMemoryDatabase($"CivilEngineerCMS-{DateTime.Now.Ticks}").Options;
    //        return new CivilEngineerCmsDbContext(optionsBuilder);
    //    }
    //}

        public static CivilEngineerCmsDbContext MockDatabase()
        {
            DbContextOptionsBuilder<CivilEngineerCmsDbContext> optionsBuilder
                = new DbContextOptionsBuilder<CivilEngineerCmsDbContext>();

            optionsBuilder.UseInMemoryDatabase($"CivilEngineerCMS-{DateTime.Now.Ticks}");

            return new CivilEngineerCmsDbContext(optionsBuilder.Options);
        }
}