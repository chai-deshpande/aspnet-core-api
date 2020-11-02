using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Student.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
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

      // load SeriLog configuration from appsettings - this requires building
      // a static, one-off instance of ConfigurationBuilder so that we can
      // wrap the entire program in a try/catch block for logging
      var configuration = new ConfigurationBuilder()
        .AddJsonFile("appSettings.json")
        .AddJsonFile($"appSettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
        .AddCommandLine(args) // just in case you want to override to run from console
        .Build();

      var loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();

      // Create Serilog logger (using the extension method)
      Log.Logger = loggerConfiguration.CreateLogger();
      try
      {
        Log.Information("Starting web host");
        CreateHostBuilder(args).Build().Run();
      }
      catch (Exception exception)
      {
        Log.Fatal(exception, "Application start-up failed");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
        .UseSerilog();
  }
}