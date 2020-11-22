using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Cosmos;
using Azure.Cosmos.Fluent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using StackExchange.Exceptional;
using Student.Api.Repositories;

namespace Student.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddMvc(option => option.EnableEndpointRouting = false);

      var exceptionsSection = Configuration.GetSection("Exceptional");

      // Make IOptions<ExceptionalSettings> available for injection everywhere
      services.AddExceptional(exceptionsSection, settings =>
      {
        settings.UseExceptionalPageOnThrow = Environment.IsDevelopment();
      });

      // Add the CosmosClient with Singleton scope
      services.AddSingleton((s) => CreateCosmosClientInstance(Configuration));
      services.AddSingleton<IStudentDocumentRepository, StudentDocumentRepository>();
    }

    private static CosmosClient CreateCosmosClientInstance(IConfiguration configuration)
    {
      string connectionString = configuration["connectionStrings:documentdb"];
      if (string.IsNullOrEmpty(connectionString))
      {
        throw new ArgumentNullException("Please specify a valid connection sting for the document db in appSettings.json");
      }

      return new CosmosClientBuilder(connectionString).Build();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
      //new Exception("Startup test exception - see how I'm captured! This happens due to a pre-.Configure() IStartupFilter").LogNoContext();
      // Boilerplate we're no longer using with Exceptional
      //if (env.IsDevelopment())
      //{
      //    app.UseDeveloperExceptionPage();
      //    app.UseBrowserLink();
      //}
      //else
      //{
      //    app.UseExceptionHandler("/Home/Error");
      //}

      app.UseSerilogRequestLogging();

      app.UseExceptional();

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
