using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Models.DataProviders
{

    public interface IDataProviderLoader
    {

        IEnumerable<IDataSource> GetAllDataSources();

        IDataSource GetDataSourceByGuid(string guid);

    }

    public class DataProviderLoader : IDataProviderLoader
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DataProviderLoader(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<IDataSource> GetAllDataSources()
        {
            List<IDataSource> dataSources = new List<IDataSource>();
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            foreach(string dll in Directory.GetFiles(Assembly.GetEntryAssembly()
                                                             ?.Location, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                foreach(Type type in assembly.GetTypes())
                {
                    if(type.GetInterface("IDataSource") != typeof(IDataSource)) continue;
                    IDataSource dataSource = scope.ServiceProvider.GetService(type) as IDataSource;
                    dataSources.Add(dataSource);
                }
            }

            return dataSources;
        }

        public IDataSource GetDataSourceByGuid(string guid)
        {
            return GetAllDataSources()
                .SingleOrDefault(d => d.Guid == guid);
        }

    }

}
