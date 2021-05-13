using JW.KS.API.Data;
using Microsoft.EntityFrameworkCore;

namespace JW.KS.API.UnitTest
{
    public class InMemoryDbContextFactory
    {
        public ApplicationDbContext GetApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryApplicationDatabase")
                .Options;
            var dbContext = new ApplicationDbContext(options);

            return dbContext;
        }
    }
}