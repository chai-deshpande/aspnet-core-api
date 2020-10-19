using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Student.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
          //.ConfigureAppConfiguration((hostingContext, config) =>
          //{
          //  config.Sources.Clear();

          //  var env = hostingContext.HostingEnvironment;

          //  config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", 
          //      optional: false, reloadOnChange: true);

          //  config.AddEnvironmentVariables();

          //  if (args != null)
          //  {
          //    config.AddCommandLine(args);
          //  }
          //})
          .ConfigureWebHostDefaults(webBuilder =>
          {
            webBuilder.UseStartup<Startup>();
          });
  }
}
