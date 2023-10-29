using HealthERP.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HealthERP.Test.Commons
{
    public class IntegrationTestDatabaseGenerator
    {
        public IntegrationTestDatabaseGenerator()
        {
           
        }

        public AppDbContext Generate()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                                .UseInMemoryDatabase(databaseName: "mytestdb")
                                .Options;

            var newDbContext = new AppDbContext(options);
            newDbContext.Database.EnsureCreated();

            return newDbContext;
        }
    }  
}
