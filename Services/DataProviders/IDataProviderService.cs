using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.DataProviders
{

    public interface IDataProviderService
    {

        Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string accessToken);

        Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id);

        bool IsExistingDataSourceGuid(string dataSourceGuid);

    }

}
