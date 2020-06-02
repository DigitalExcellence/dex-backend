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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sentry;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace IdentityServer
{

    public class Program
    {

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
                             if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                             {
                                 IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                                 s.Dsn = new Dsn(config.GetSection("App:Sentry:IdentityDsn").Value);
                             }
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                           webBuilder.UseSentry();
                           webBuilder.UseSerilog();
                       });
        }

    }

}
