using HealthERP.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HealthERP.Presentation.Extensions
{
    public static class DatabaseExtensionService
    {
        public static IServiceCollection AddDatabaseService(this IServiceCollection Services, IConfiguration configuration)
        {
            var connection = GetSqliteConnection(configuration);

            Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlite(connection);
            });

            return Services;
        }

        public static SqliteConnection GetSqliteConnection(IConfiguration configuration)
        {
            var dbname = configuration.GetSection("Database")["dev-db"];
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = dbname };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            return connection;
        }
    }
}
