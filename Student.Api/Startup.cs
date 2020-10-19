using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Exceptional;

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

      // Make IOptions<ExceptionalSettings> available for injection everywhere
      services.AddExceptional(Configuration.GetSection("Exceptional"), settings =>
      {
        //settings.DefaultStore.ApplicationName = "Samples.AspNetCore";
        settings.UseExceptionalPageOnThrow = Environment.IsDevelopment();
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
      new Exception("Startup test exception - see how I'm captured! This happens due to a pre-.Configure() IStartupFilter").LogNoContext();
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
      app.UseExceptional();

      //if (Environment.IsDevelopment())
      //{
      //  app.UseDeveloperExceptionPage();
      //}

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
