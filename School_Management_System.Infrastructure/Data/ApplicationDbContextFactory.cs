using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace School_Management_System.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "School_Management_System");

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
}