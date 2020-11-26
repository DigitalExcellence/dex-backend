using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{

    public interface IDataSource
    {
        string Guid { get; }

        string Name { get; }
        string OauthUrl { get; }

        Task<OauthTokens> GetTokens(string code);

        Task<IEnumerable<Project>> GetAllProjects(string accessToken);

    }

}
