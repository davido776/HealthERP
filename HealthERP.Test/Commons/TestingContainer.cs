using HealthERP.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthERP.Test.Commons
{
    public class TestingContainer : IDisposable
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private string dbName = "TestDatabase.db";

        public TestingContainer()
        {
            Connection = new SqliteConnection($"Filename={dbName}");

            Connection.Open();

            EnsureDatabaseCreated();
        }

        public SqliteConnection Connection { get; }

        public AppDbContext CreateContext(DbTransaction? transaction = null)
        {
            //var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "TestDatabase.db" };
            //var connectionString = connectionStringBuilder.ToString();
            //vConnection = new SqliteConnection(connectionString);

            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(Connection).Options);
            
            if (transaction != null)
            {
                
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void EnsureDatabaseCreated()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureCreated();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}
