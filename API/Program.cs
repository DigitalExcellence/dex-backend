/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;

namespace API
{
    /// <summary>
    ///     Program.cs
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code of the program.</returns>
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                         .MinimumLevel.Override("System", LogEventLevel.Warning)
                         .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                         .Enrich.FromLogContext()
                         .WriteTo.Sentry(s =>
                         {
                             s.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                             s.MinimumEventLevel = LogEventLevel.Error;
                         })
                         .WriteTo.Console(outputTemplate:
                                          "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                                          theme: AnsiConsoleTheme.Literate)
                         .CreateLogger();

            try
            {
                Log.Information("Starting host...");
                CreateHostBuilder(args)
                    .Build()
                    .Run();
                return 0;
            } catch(Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            } finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The webhostbuilder instance.</returns>
        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseContentRoot(Directory.GetCurrentDirectory())
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              IWebHostEnvironment env = hostingContext.HostingEnvironment;
                              config.AddJsonFile("appsettingsapi.json", true, true)
                                    .AddJsonFile($"appsettingsapi.{env.EnvironmentName}.json", true, true);
                              config.AddEnvironmentVariables();
                          })
                          .UseSentry()
                          .UseStartup<Startup>()
                          .UseKestrel(o => o.AddServerHeader = false)
                          .UseSerilog();
        }
    }
}
