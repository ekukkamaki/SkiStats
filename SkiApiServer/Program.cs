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


        // seed excel data
        var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        //esa
        var person = new Entities.Models.Person
        {
          Name = "Esa Kukkamäki",
          Age = 38
        };
        excelLoader.ExcelHandler.SeedDbForExcelData(context, "ESAYHT_hiihto.xls", person);
        //eero
        person = new Entities.Models.Person
        {
          Name = "Eero Kukkamäki",
          Age = 69
        };
        excelLoader.ExcelHandler.SeedDbForExcelData(context, "EEROYHT.xls", person);
      };

      return host;
    }
  }
}
