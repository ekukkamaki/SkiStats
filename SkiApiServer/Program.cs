using Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SkiApiServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .MigrateDb()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static IWebHost MigrateDb(this IWebHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                // Create database if required and ensure it is at a new enough version
                scope.ServiceProvider.GetRequiredService<RepositoryContext>().Database.Migrate();
            }

            return host;
        }
    }
}
