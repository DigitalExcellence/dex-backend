using Microsoft.Extensions.Configuration;

namespace Models
{

    public class GithubDataSource : IDataSource
    {

        public GithubDataSource(IConfiguration configuration)
        {
            ClientId = configuration.GetSection($"{Name}ClientId")
                                    .Value;
            ClientSecret = configuration.GetSection($"{Name}ClientSecret")
                                        .Value;

            OauthUrl = "https://github.com/login/oauth/authorize?client_id=" + ClientId + $"&scope=repo&state={Name}";
        }

        public string Guid { get; } = "de38e528-1d6d-40e7-83b9-4334c51c19be";

        public string Name { get; } = "Github";

        public string BaseUrl { get; } = "https://api.github.com/";

        public string OauthUrl { get; } 

        public string ClientId { get; }

        public string ClientSecret { get; }

    }

}
