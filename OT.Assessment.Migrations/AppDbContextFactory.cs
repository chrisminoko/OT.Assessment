using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OT.Assessment.Migrations
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Look in current directory
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection")
                ?? throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");

            optionsBuilder.UseSqlServer(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
