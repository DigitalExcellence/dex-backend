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

using IdentityModel.Client;
using MessageBrokerPublisher;
using MessageBrokerPublisher.HelperClasses;
using MessageBrokerPublisher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;

namespace JobScheduler
{

    /// <summary>
    ///     Program.cs
    /// </summary>
    public class Program
    {

        /// <summary>
        ///     Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The exit code of the program.</returns>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        /// <summary>
        ///     Creates the host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The webhostbuilder instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                                                         .AddJsonFile("appsettingsjobscheduler.json", true, true)
                                                         .AddJsonFile($"appsettingsjobscheduler.{environmentName}.json",
                                                                      true,
                                                                      true)
                                                         .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            Config config = configuration.GetSection("Config")
                                         .Get<Config>();


            return Host.CreateDefaultBuilder(args)
                       .ConfigureAppConfiguration(builder => configurationBuilder.Build())
                       .ConfigureServices((hostContext, services) =>
                       {
                           services.AddAccessTokenManagement(options =>
                                   {
                                       options.Client.Clients.Add("identityserver",
                                                                  new ClientCredentialsTokenRequest
                                                                  {
                                                                      Address =
                                                                          config.IdentityServerConfig.IdentityUrl +
                                                                          "connect/token",
                                                                      ClientId = config.IdentityServerConfig.ClientId,
                                                                      ClientSecret =
                                                                          config.IdentityServerConfig.ClientSecret
                                                                  });
                                   })
                                   .ConfigureBackchannelHttpClient()
                                   .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
                                                                    {
                                                                        TimeSpan.FromSeconds(1),
                                                                        TimeSpan.FromSeconds(2),
                                                                        TimeSpan.FromSeconds(3)
                                                                    }));
                           services.AddClientAccessTokenClient("client",
                                                               configureClient: client =>
                                                               {
                                                                   client.BaseAddress =
                                                                       new Uri(config.ApiConfig.ApiUrl);
                                                               });
                           services.AddScoped<IRabbitMQConnectionFactory>(c => new RabbitMQConnectionFactory(
                                                                              config.RabbitMQ.Hostname,
                                                                              config.RabbitMQ.Username,
                                                                              config.RabbitMQ.Password));
                           services.AddScoped<ITaskPublisher, TaskPublisher>();
                           services.AddScoped<IEmailSender, EmailSender>();
                           services.AddScoped<IApiRequestHandler, ApiRequestHandler>();
                           services.AddHostedService<GraduationWorker>();
                           services.AddHostedService<AlgorithmWorker>();
                           services.AddSingleton(config);
                       });
        }

    }

}
