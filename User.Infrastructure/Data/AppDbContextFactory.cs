using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace User.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string? currentDir = Directory.GetCurrentDirectory();
            string? rootPath = Path.GetDirectoryName(currentDir);
            string lastDir = "BackEnd.User.Api";
            string basePath = Path.Combine(rootPath ?? "", lastDir);

            var configuration = new ConfigurationBuilder().SetBasePath("").AddJsonFile("appsettings.json", optional: true).Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}