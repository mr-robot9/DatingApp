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
using Serilog.Events;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
                .CreateLogger();

            try{
                Log.Information("Starting Host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch{
                Log.Fatal("Host Terminated Unexpectedly");
            }
            finally{
                Log.CloseAndFlush();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseStartup<Startup>();
    }
}
