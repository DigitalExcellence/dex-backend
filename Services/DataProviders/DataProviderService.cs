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
            IEnumerable<Project> projects = await dataSource.GetAllProjects(accessToken);
            return projects;
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

        public async Task<OauthTokens> GetTokens(string code, string guid)
        {
            IDataSource dataProvider = dataProviderLoader.GetDataSourceByGuid(guid);
            OauthTokens tokens = await dataProvider.GetTokens(code);
            return tokens;
        }

    }

}
