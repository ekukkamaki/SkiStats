using Contracts;
using Entities;
using ExternalServices.Fmi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Microsoft.OpenApi.Models;

namespace SkiApiServer
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.AddDbContext<RepositoryContext>(c =>
      {
        c.UseSqlServer(Configuration["ConnectionStrings:skiStat"], b => b.MigrationsAssembly("SkiApiServer"));
      });
      //services.AddDbContext<RepositoryContext>(opt => opt.UseInMemoryDatabase("SkiStatistics"));

      services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddHttpClient<FmiService>();

      services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ski API", Version = "v1" });
    });


      services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                 options.Authority = "http://localhost:5001";
                 options.RequireHttpsMetadata = false;

                 options.Audience = "api1";
               });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      //app.UseHttpsRedirection();
      app.UseAuthentication();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("swagger/v1/swagger.json", "Ski Api V1");
        c.RoutePrefix = string.Empty;
      });

      app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
      app.UseMvc();

    }
  }
}
