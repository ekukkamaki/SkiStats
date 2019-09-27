using System.Linq;
using System.Reflection;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SkiApiServer;

namespace IdentityServer
{
  public class Startup
  {

    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      const string connectionString = @"Data Source=.\SQLEXPRESS;database=IdentityServer;trusted_connection=True;MultipleActiveResultSets=true";
      var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;


      services.AddCors(options =>
      {
        options.AddPolicy(MyAllowSpecificOrigins, builder =>
            {
              builder.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
            });
      });

      services.AddIdentityServer()
          .AddDeveloperSigningCredential()
          .AddTestUsers(Config.GetUsers())
          //config data(clients, resources)
          .AddConfigurationStore(options =>
          {
            options.ConfigureDbContext = builder =>
                      builder.UseSqlServer(connectionString,
                          sql => sql.MigrationsAssembly(migrationsAssembly));
          })
          //operational data, such as (codes, tokens, consents)
          .AddOperationalStore(options =>
          {
            options.ConfigureDbContext = builder =>
                      builder.UseSqlServer(connectionString,
                          sql => sql.MigrationsAssembly(migrationsAssembly));
            options.EnableTokenCleanup = true;
            options.TokenCleanupInterval = 30;
          });



      services.AddAuthentication()
          .AddOpenIdConnect("oidc", "OpenID Connect", options =>
          {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            options.SignOutScheme = IdentityServerConstants.SignoutScheme;
            options.SaveTokens = true;

            options.Authority = "https://demo.identityserver.io/";
            options.ClientId = "implicit";

            options.TokenValidationParameters = new TokenValidationParameters
            {
              NameClaimType = "name",
              RoleClaimType = "role"
            };
          })
           .AddGoogle("Google", options =>
          {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            options.ClientId = "724969426796-ti0tk5qmcp25niaieom8cnlelhusgc7u.apps.googleusercontent.com";
            options.ClientSecret = "-Tcp9XIYcGHRy705zaH6fbiq";
          });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      // this will do the initial DB population
      InitializeDatabase(app);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseStaticFiles();

      app.UseIdentityServer();

      app.UseMvcWithDefaultRoute();
    }
    private void InitializeDatabase(IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();
        if (!context.Clients.Any())
        {
          foreach (var client in Config.GetClients())
          {
            context.Clients.Add(client.ToEntity());
          }
          context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
          foreach (var resource in Config.GetIdentityResources())
          {
            context.IdentityResources.Add(resource.ToEntity());
          }
          context.SaveChanges();
        }

        if (!context.ApiResources.Any())
        {
          foreach (var resource in Config.GetApis())
          {
            context.ApiResources.Add(resource.ToEntity());
          }
          context.SaveChanges();
        }
      }
    }
  }
}
