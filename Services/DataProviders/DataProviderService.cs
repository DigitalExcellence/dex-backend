using AngleSharp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DataProviders
{

    public class DataProviderService : IDataProviderService
    {

        private readonly IDataProviderLoader dataProviderLoader;

        public DataProviderService(IDataProviderLoader dataProviderLoader)
        {
            this.dataProviderLoader = dataProviderLoader;
        }

        public async Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string accessToken)
        {
            IDataSource dataSource = dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);

            RestClient client = new RestClient(dataSource.BaseUrl + "user/repos");
            client.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
            RestRequest request = new RestRequest(Method.GET);
            request.AddQueryParameter("visibility", "all");
            request.AddHeader("accept", "application/vnd.github.v3+json");

            IEnumerable<Project> response = (await client.ExecuteAsync<IEnumerable<Project>>(request)).Data;
            return response;
        }

        public async Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id)
        {
            IEnumerable<Project> projects = await GetAllProjects(dataSourceGuid, accessToken);
            return projects.SingleOrDefault(p => p.Id == id);
        }

        public bool IsExistingDataSourceGuid(string dataSourceGuid)
        {
            return dataProviderLoader.GetDataSourceByGuid(dataSourceGuid) != null;
        }

        public string GetOauthUrl(string guid)
        {
            return dataProviderLoader.GetDataSourceByGuid(guid)
                                     .OauthUrl;
        }

        public Task<OauthTokens> GetTokens(string code, string guid)
        {
            throw new NotImplementedException();
        }

    }

}
