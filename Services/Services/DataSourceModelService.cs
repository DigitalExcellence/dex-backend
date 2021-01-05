using Models;
using Repositories;
using Repositories.Base;
using Services.Base;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IDataSourceModelService : IService<DataSource>
    {

        Task<DataSource> GetDataSourceByGuid(string guid);

    }

    public class DataSourceModelService : Service<DataSource>, IDataSourceModelService
    {

        public DataSourceModelService(IRepository<DataSource> repository) : base(repository) { }

        protected new IDataSourceModelRepository Repository => (IDataSourceModelRepository) base.Repository;

        public async Task<DataSource> GetDataSourceByGuid(string guid)
        {
            return await Repository.GetDataSourceByGuid(guid);
        }

    }

}
