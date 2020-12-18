using AutoMapper;
using Models;
using Newtonsoft.Json;
using RestSharp;
using Services.DataProviders.Resources;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public class JsFiddleDataSourceAdaptee : IPublicDataSourceAdaptee
    {
        /// <summary>
        /// A factory that will generate a rest client to make API requests.
        /// </summary>
        private readonly IRestClientFactory restClientFactory;

        /// <summary>
        /// Mapper object from auto mapper that will automatically maps one object to another.
        /// </summary>
        private readonly IMapper mapper;

        public JsFiddleDataSourceAdaptee(IRestClientFactory restClientFactory, IMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
        }

        public string Guid { get; }

        public string Name { get; }

        public string BaseUrl { get; }

        public async Task<IEnumerable<Project>> GetAllPublicProjects(string username)
        {
            JsFiddleDataSourceResourceResult resourceResult = await FetchAllFiddlesFromUser(username);
            if(resourceResult.OverallResultSetCount <= 0) return null;
            return mapper.Map<JsFiddleDataSourceResourceResult, IEnumerable<Project>>(resourceResult);
        }

        private async Task<JsFiddleDataSourceResourceResult> FetchAllFiddlesFromUser(string username)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"/user/{username}/demo/list.json", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(response.StatusCode != HttpStatusCode.OK ||
               string.IsNullOrEmpty(response.Content))
            {
                return null;
            }

            JsFiddleDataSourceResourceResult resourceResult =
                JsonConvert.DeserializeObject<JsFiddleDataSourceResourceResult>(response.Content);
            return resourceResult;
        }

        public Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            return null;
        }

    }

}
