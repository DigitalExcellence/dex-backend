using System;
using System.Collections.Generic;

namespace Models.DataProviders
{

    public interface IDataProviderLoader
    {

        IEnumerable<IDataSource> GetAllDataSources();

        IDataSource GetDataSourceByGuid(string guid);

    }

    public class DataProviderLoader : IDataProviderLoader
    {

        public IEnumerable<IDataSource> GetAllDataSources()
        {
            throw new NotImplementedException();
        }

        public IDataSource GetDataSourceByGuid(string guid)
        {
            throw new NotImplementedException();
        }

    }

}
