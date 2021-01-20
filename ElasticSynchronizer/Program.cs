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

using ElasticSynchronizer.Configuration;
using ElasticSynchronizer.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace ElasticSynchronizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>


            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    string environmentName = Environment.GetEnvironmentVariable("ELASTIC_DOTNET_ENVIRONMENT");

                    IConfiguration configuration = new ConfigurationBuilder()
                                                   .AddJsonFile("appsettings.json", true, true)
                                                   .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                                                   .AddEnvironmentVariables()
                                                   .Build();

                    Config config = configuration.GetSection("App").Get<Config>();
                    UriBuilder builder = new UriBuilder("http://" + config.Elastic.Hostname + ":9200/");
                    Uri uri = builder.Uri;

                    services.AddScoped( c => config);
                    services.AddScoped(client => new RestClient(uri)
                    {
                        Authenticator =
                                 new HttpBasicAuthenticator(config.Elastic.Username, config.Elastic.Password)
                    });

                    services.AddHostedService<DeleteProjectWorker>();
                    services.AddHostedService<UpdateProjectWorker>();
                });
    }
}
