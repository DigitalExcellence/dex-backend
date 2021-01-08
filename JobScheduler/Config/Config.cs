using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using Serilog;
using System;

namespace JobScheduler
{
    public class Config
    {
        public IdentityServerConfig IdentityServerConfig { get; set; }
        public ApiConfig ApiConfig { get; set; }


        public Config(IConfiguration config)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            config = new ConfigurationBuilder()
                                           .AddJsonFile("appsettingsjobscheduler.json", true, true)
                                           .AddJsonFile($"appsettingsjobscheduler.{environmentName}.json", true, true)
                                           .AddEnvironmentVariables()
                                           .Build();
            IdentityServerConfig  = config.GetSection("App")
                                          .GetSection("IdentityServerConfig")
                                          .Get<IdentityServerConfig>();
            ApiConfig = config.GetSection("App")
                              .GetSection("API")
                              .Get<ApiConfig>();
        }

    }

    public class IdentityServerConfig
    {
        public string IdentityUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class ApiConfig
    {
        public string ApiUrl { get; set; }
    }


}
