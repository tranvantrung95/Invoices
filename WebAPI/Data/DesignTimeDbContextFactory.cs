using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebAPI.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InvoicikaDbContext>
    {
        public InvoicikaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<InvoicikaDbContext>();
            var databaseProvider = configuration["DatabaseProvider"];

            if (databaseProvider == "SqlServer")
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            }
            else if (databaseProvider == "MySql")
            {
                optionsBuilder.UseMySql(configuration.GetConnectionString("MySqlConnection"), new MySqlServerVersion(new Version(8, 0, 26)));
            }

            return new InvoicikaDbContext(optionsBuilder.Options);
        }
    }
}
