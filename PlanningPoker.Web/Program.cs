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

namespace PlanningPoker.Web {
    public class Program {
        public static void Main (string[] args) {
            Log.Logger = new LoggerConfiguration ()
                .MinimumLevel.Debug ()
                .MinimumLevel.Override ("Microsoft", LogEventLevel.Verbose)
                .Enrich.FromLogContext ()
                .WriteTo.RollingFile("./logs/log-{Date}.txt")
                .CreateLogger ();

            try {
                Log.Information ("Starting web host");

                BuildWebHost (args).Run ();
            } catch (Exception ex) {
                Log.Fatal (ex, "Host terminated unexpectedly");
            } finally {
                Log.CloseAndFlush ();
            }

        }

        public static IWebHost BuildWebHost (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .ConfigureLogging ((context, factory) => {
                factory.AddConfiguration (context.Configuration.GetSection ("Logging"));
                factory.AddConsole ();
                factory.AddDebug ();
            })
            .UseKestrel ()
            .UseStartup<Startup> ()
            .Build ();
    }
}
