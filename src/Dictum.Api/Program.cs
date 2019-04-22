using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Dictum.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
              .AddJsonFile($"appsettings.{currentEnv}.json", optional: true, reloadOnChange: false)
              .AddEnvironmentVariables()
              .Build();

            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(configuration)
              .CreateLogger();
            try
            {
                Log.Information("Starting Monitoring Web Api Host");
                CreateWebHostBuilder(args).Build().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("hosting.json", optional: true)
              .AddCommandLine(args)
              .Build();

            return WebHost.CreateDefaultBuilder(args)
                .SuppressStatusMessages(true)
              //.UseUrls("http://*:5000")
              .UseConfiguration(config)
              .UseStartup<Startup>()
              .UseSerilog();
        }
    }
}
