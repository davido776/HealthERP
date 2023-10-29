using HealthERP.Domain.Claims;
using HealthERP.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthERP.Tests.Commons
{
    public class TestingContainer :IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private string dbName = "TestDatabase.db";

        public TestingContainer()
        {
            Connection = new SqliteConnection($"Filename={dbName}");

            Connection.Open();
        }

        public DbConnection Connection { get; }

        public AppDbContext CreateContext(DbTransaction? transaction = null)
        {
            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(Connection).Options);

            if (transaction != null)
            {
                context.Database.MigrateAsync().Wait();
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        public void Dispose() => Connection.Dispose();
    }
}
