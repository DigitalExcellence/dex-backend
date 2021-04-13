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

using Microsoft.Extensions.Configuration;
using NotificationSystem.Configuration;
using NotificationSystem.Contracts;
using NotificationSystem.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SendGrid;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace NotificationSystem
{

    internal class Program
    {

        private static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                         .MinimumLevel.Override("System", LogEventLevel.Warning)
                         .Enrich.FromLogContext()
                         .WriteTo.Console(outputTemplate:
                                          "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                                          theme: AnsiConsoleTheme.Literate)
                         .CreateLogger();

            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Appsettings is renamed because of an issue where the project loaded another appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                                           .AddJsonFile("appsettingsnotificationsystem.json", true, true)
                                           .AddJsonFile($"appsettingsnotificationsystem.{environmentName}.json",
                                                        true,
                                                        true)
                                           .AddEnvironmentVariables()
                                           .Build();
            Config config = configuration.GetSection("App")
                                         .Get<Config>();

            IRabbitMQConnectionFactory connectionFactory =
                new RabbitMQConnectionFactory(config.RabbitMQ.Hostname,
                                              config.RabbitMQ.Username,
                                              config.RabbitMQ.Password);

            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(connectionFactory);
            IModel channel = subscriber.SubscribeToSubject("EMAIL");

            RabbitMQListener listener = new RabbitMQListener(channel);

            // inject your notification service here
            ISendGridClient sendGridClient = new SendGridClient(config.SendGrid.ApiKey);
            ICallbackService notificationService = new EmailSender(sendGridClient, config.SendGrid.EmailFrom, config.SendGrid.SandboxMode);
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);

            listener.StartConsumer(consumer, "EMAIL");
            Console.ReadLine();
        }

    }

}
