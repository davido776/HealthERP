using HealthERP.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HealthERP.Test.Commons
{
    public class IntegrationTestDatabaseGenerator
    {
        public AppDbContext Generate()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "test.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            var options2 = new DbContextOptionsBuilder<AppDbContext>()
                                .UseInMemoryDatabase(databaseName: "mytestdb")
                                .Options;

            //var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            var newDbContext = new AppDbContext(options2);
            newDbContext.Database.EnsureCreated();

            return newDbContext;
        }
    }  
}
